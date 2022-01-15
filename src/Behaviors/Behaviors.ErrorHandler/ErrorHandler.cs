using Rhyous.BusinessRules;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Authentication;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class ErrorHandler : IErrorHandler
    {
        internal ILogger Logger;
        public ErrorHandler(ILogger logger)
        {
            Logger = logger;
        }

        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // Retun serialization errors as 
            if (error is AuthenticationException)
                fault = CreateResponse(error, version, HttpStatusCode.Unauthorized);
            else if (error is SerializationException)
                fault = CreateResponse(error, version, HttpStatusCode.BadRequest);
            else if (error is RestException)
                fault = CreateResponse(error, version, (error as RestException).StatusCode);
            else if (error is ServiceErrorResponseForwarderException)
                fault = CreateResponse((error as ServiceErrorResponseForwarderException).Response, version, (error as ServiceErrorResponseForwarderException).Code);
            else
                fault = CreateResponse(error, version, HttpStatusCode.InternalServerError);

            Logger?.Write(error);
        }

        internal Message CreateResponse(Exception error, MessageVersion version, HttpStatusCode statusCode, string messageOverride = null)
        {
            bool isAck = AcknowledgeableTypes.Contains(error.GetType().ToString());
            var errorResponse = new ServiceErrorResponse(error, isAck, messageOverride);
            return CreateResponse(errorResponse, version, statusCode);
        }

        internal Message CreateResponse(ServiceErrorResponse errorResponse, MessageVersion version, HttpStatusCode statusCode)
        {
            var json = new Serializer().Json(errorResponse);
            Logger?.Write(new UTF8Encoding(false).GetString(json));
            var body = new RawBodyWriter(json);

            var httpResponseMessageProperty = new HttpResponseMessageProperty
            {
                StatusCode = statusCode,
                StatusDescription = "See ServiceErrorResponse object for more information."
            };
            var faultMessage = MessageBuilder.Build(version, "", body, httpResponseMessageProperty);
            return faultMessage;
        }

        /// <summary>
        /// This is configuration in code. We want to in the future populate this list from a database entity.
        /// </summary>
        internal static List<string> AcknowledgeableTypes = new List<string>
        {
            typeof(BusinessRulesNotMetException).ToString(),
            typeof(AuthenticationException).ToString()
        };

        public IMessageBuilder MessageBuilder
        {
            get { return _MessageBuilder ?? (_MessageBuilder = new MessageBuilder()); }
            set { _MessageBuilder = value; }
        } private IMessageBuilder _MessageBuilder;

    }
}