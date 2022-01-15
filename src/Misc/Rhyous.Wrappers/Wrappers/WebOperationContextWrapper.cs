using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Rhyous.Wrappers
{
    public class WebOperationContextWrapper : IWebOperationContext
    {
        public IncomingWebResponseContext IncomingWebResponseContext;

        public WebOperationContextWrapper(WebOperationContext context)
        {
            Context = context ?? throw new ArgumentNullException("context");
        }
        public WebOperationContext Context { get; }

        public IIncomingWebResponseContext IncomingResponse => new IncomingWebResponseContextWrapper(Context.IncomingResponse);

        public IIncomingWebRequestContext IncomingRequest => new IncomingWebRequestContextWrapper(Context.IncomingRequest);

        public IOutgoingWebRequestContext OutgoingRequest => new OutgoingWebRequestContextWrapper(Context.OutgoingRequest);

        public IOutgoingWebResponseContext OutgoingResponse => new OutgoingWebResponseContextWrapper(Context.OutgoingResponse);


        public void Attach(OperationContext owner) => Context.Attach(owner);

        public Message CreateAtom10Response(SyndicationItem item) => Context.CreateAtom10Response(item);

        public Message CreateAtom10Response(SyndicationFeed feed) => Context.CreateAtom10Response(feed);

        public Message CreateAtom10Response(ServiceDocument document) => Context.CreateAtom10Response(document);

        public Message CreateJsonResponse<T>(T instance) => Context.CreateJsonResponse(instance);

        public Message CreateJsonResponse<T>(T instance, DataContractJsonSerializer serializer) => Context.CreateJsonResponse(instance, serializer);

        public Message CreateStreamResponse(Action<Stream> streamWriter, string contentType) => Context.CreateStreamResponse(streamWriter, contentType);

        public Message CreateStreamResponse(StreamBodyWriter bodyWriter, string contentType) => Context.CreateStreamResponse(bodyWriter, contentType);

        public Message CreateStreamResponse(Stream stream, string contentType) => Context.CreateStreamResponse(stream, contentType);

        public Message CreateTextResponse(Action<TextWriter> textWriter, string contentType) => Context.CreateTextResponse(textWriter, contentType);

        public Message CreateTextResponse(Action<TextWriter> textWriter, string contentType, Encoding encoding) => Context.CreateTextResponse(textWriter, contentType, encoding);

        public Message CreateTextResponse(string text) => Context.CreateTextResponse(text);

        public Message CreateTextResponse(string text, string contentType, Encoding encoding) => Context.CreateTextResponse(text, contentType, encoding);

        public Message CreateTextResponse(string text, string contentType) => Context.CreateTextResponse(text, contentType);

        public Message CreateXmlResponse(XElement element) => Context.CreateXmlResponse(element);

        public Message CreateXmlResponse(XDocument document) => Context.CreateXmlResponse(document);

        public Message CreateXmlResponse<T>(T instance, XmlSerializer serializer) => Context.CreateXmlResponse(instance, serializer);

        public Message CreateXmlResponse<T>(T instance) => Context.CreateXmlResponse(instance);

        public Message CreateXmlResponse<T>(T instance, XmlObjectSerializer serializer) => Context.CreateXmlResponse(instance, serializer);

        public void Detach(OperationContext owner) => Context.Detach(owner);

        public UriTemplate GetUriTemplate(string operationName) => Context.GetUriTemplate(operationName);
    }
}
