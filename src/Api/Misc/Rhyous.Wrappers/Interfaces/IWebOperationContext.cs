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
    public interface IWebOperationContext
    {
        WebOperationContext Context { get; }
        IIncomingWebResponseContext IncomingResponse { get; }
        IIncomingWebRequestContext IncomingRequest { get; }
        IOutgoingWebRequestContext OutgoingRequest { get; }
        IOutgoingWebResponseContext OutgoingResponse { get; }

        void Attach(OperationContext owner);
        Message CreateAtom10Response(SyndicationItem item);
        Message CreateAtom10Response(SyndicationFeed feed);
        Message CreateAtom10Response(ServiceDocument document);
        Message CreateJsonResponse<T>(T instance);
        Message CreateJsonResponse<T>(T instance, DataContractJsonSerializer serializer);
        Message CreateStreamResponse(Action<Stream> streamWriter, string contentType);
        Message CreateStreamResponse(StreamBodyWriter bodyWriter, string contentType);
        Message CreateStreamResponse(Stream stream, string contentType);
        Message CreateTextResponse(Action<TextWriter> textWriter, string contentType);
        Message CreateTextResponse(Action<TextWriter> textWriter, string contentType, Encoding encoding);
        Message CreateTextResponse(string text);
        Message CreateTextResponse(string text, string contentType, Encoding encoding);
        Message CreateTextResponse(string text, string contentType);
        Message CreateXmlResponse(XElement element);
        Message CreateXmlResponse(XDocument document);
        Message CreateXmlResponse<T>(T instance, XmlSerializer serializer);
        Message CreateXmlResponse<T>(T instance);
        Message CreateXmlResponse<T>(T instance, XmlObjectSerializer serializer);
        void Detach(OperationContext owner);
        UriTemplate GetUriTemplate(string operationName);
    }
}
