using Rhyous.BusinessRules;
using System;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Authentication;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Rhyous.WebFramework.Behaviors
{
    public class ErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            // Retun serialization errors as 
            if (error is AuthenticationException)
                fault = CreateResponse(error, version, HttpStatusCode.Unauthorized, ((ServiceResponseSection)ConfigurationManager.GetSection("ServiceResponseSection")).GetValue("AuthFailure"));
            else if (error is SerializationException)
                fault = CreateResponse(error, version, HttpStatusCode.BadRequest);
            else
                fault = CreateResponse(error, version, HttpStatusCode.InternalServerError);
        }

        private static Message CreateResponse(Exception error, MessageVersion version, HttpStatusCode statusCode, string messageOverride = null)
        {
            var errorResponse = new ServiceErrorResponse(error, messageOverride);
            var serializer = new DataContractJsonSerializer(typeof(ServiceErrorResponse), new[] { typeof(BusinessRuleResult) });
            var fault = Message.CreateMessage(version, "", errorResponse, serializer);
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, new WebBodyFormatMessageProperty(WebContentFormat.Json));
            fault.Properties.Add(HttpResponseMessageProperty.Name, new HttpResponseMessageProperty
            {
                StatusCode = statusCode,
                StatusDescription = "See ServiceErrorResponse object for more information."
            });
            return fault;
        }
    }
}
