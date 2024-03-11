using System.Net;

namespace Rhyous.Wrappers
{
    public interface IIncomingWebResponseContext
    {
        long ContentLength { get; }
        string ContentType { get; }
        string ETag { get; }
        WebHeaderCollection Headers { get; }
        string Location { get; }
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
    }
}