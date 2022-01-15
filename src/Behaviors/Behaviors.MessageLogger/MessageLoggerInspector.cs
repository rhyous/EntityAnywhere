using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Rhyous.EntityAnywhere.Behaviors
{
    /// <summary>
    /// This is more a message logger, than just a PostLogger now. It can log both the message on Post and the message on reply.
    /// </summary>
    public class MessageLoggerInspector : IDispatchMessageInspector
    {
        internal IMessageHandler MessageLoggerHelper { get; }
        internal ILogger Logger { get; }
        internal IUrlFilter UrlFilter { get; }

        public MessageLoggerInspector(ILogger logger, IMessageHandler messageLoggerHelper, IUrlFilter urlFilter = null)
        {
            Logger = logger;
            MessageLoggerHelper = messageLoggerHelper;
            UrlFilter = urlFilter;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            Uri requestUri = request?.Headers?.To;
            if (!MessageLoggerHelper.LogRequestMessage)
                return requestUri;
            if (requestUri == null)
            {
                Logger.Debug("The request Uri is null");
                return requestUri;
            }
            if (!ShouldLog(UrlFilter?.RequestUrls, requestUri.AbsolutePath))
            {
                return requestUri;
            }
            var httpReq = MessageLoggerHelper.GetHttpRequestMessageProperty(request, HttpRequestMessageProperty.Name) as HttpRequestMessageProperty;
            const string requestMessageTxt = "Request Message";
            Logger.Debug($"{requestMessageTxt}: {httpReq.Method} {requestUri}");
            request = LogMessage(request, httpReq.Headers, requestMessageTxt);
            return requestUri;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (!MessageLoggerHelper.LogResponseMessage)
                return;
            Uri requestUri = correlationState as Uri;
            if (requestUri == null)
            {
                Logger.Debug("The request Uri is null");
                return;
            }
            if (!ShouldLog(UrlFilter?.ResponseUrls, requestUri.AbsolutePath))
            {
                return;
            }
            const string responseMessageTxt = "Response Message";
            var httpResponse = MessageLoggerHelper.GetHttpRequestMessageProperty(reply, HttpResponseMessageProperty.Name) as HttpResponseMessageProperty;
            Logger.Debug($"{responseMessageTxt}: {requestUri}");
            reply = LogMessage(reply, httpResponse.Headers, responseMessageTxt);
        }

        private bool ShouldLog(HashSet<string> urls, string absolutePath)
        {
            return urls == null || (urls != null && urls.Any() && (urls.Contains("All") || urls.Contains(absolutePath.Trim(new[] { '/' })) || urls.Contains(absolutePath)));
        }

        private Message LogMessage(Message message, WebHeaderCollection headers, string prefix)
        {
            var headerMessage = $"Headers:{Environment.NewLine}";
            foreach (var header in headers.AllKeys)
            {
                headerMessage += $"{header}: {headers[header]}{Environment.NewLine}";
            }
            Logger.Debug(headerMessage);

            if (!message.IsEmpty)
            {
                Logger.Debug($"{prefix}: {MessageLoggerHelper.MessageToString(ref message)}");
            }
            return message;
        }
    }
}