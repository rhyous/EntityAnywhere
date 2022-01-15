using System;
using System.Net;
using System.ServiceModel.Web;
using System.Text;

namespace Rhyous.Wrappers
{
    public interface IOutgoingWebResponseContext
    {
        string ContentType { get; set; }
        bool SuppressEntityBody { get; set; }
        string StatusDescription { get; set; }
        HttpStatusCode StatusCode { get; set; }
        string Location { get; set; }
        DateTime LastModified { get; set; }
        WebHeaderCollection Headers { get; }
        string ETag { get; set; }
        Encoding BindingWriteEncoding { get; }
        long ContentLength { get; set; }
        WebMessageFormat? Format { get; set; }

        void SetETag(Guid entityTag);
        void SetETag(long entityTag);
        void SetETag(int entityTag);
        void SetETag(string entityTag);
        void SetStatusAsCreated(Uri locationUri);
        void SetStatusAsNotFound(string description);
        void SetStatusAsNotFound();
    }
}