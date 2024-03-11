using System.Net;

namespace Rhyous.Wrappers
{
    public interface IOutgoingWebRequestContext
    {
        string Accept { get; set; }
        long ContentLength { get; set; }
        string ContentType { get; set; }
        WebHeaderCollection Headers { get; }
        string IfMatch { get; set; }
        string IfModifiedSince { get; set; }
        string IfNoneMatch { get; set; }
        string IfUnmodifiedSince { get; set; }
        string Method { get; set; }
        bool SuppressEntityBody { get; set; }
        string UserAgent { get; set; }
    }
}