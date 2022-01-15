using System;
using System.Net;
using System.ServiceModel.Web;
using System.Text;

namespace Rhyous.Wrappers
{
    public class OutgoingWebResponseContextWrapper : IOutgoingWebResponseContext
    {
        public OutgoingWebResponseContext Context;

        public OutgoingWebResponseContextWrapper(OutgoingWebResponseContext context)
        {
            Context = context ?? throw new ArgumentNullException("context");
        }

        public string ContentType { get => Context.ContentType; set => Context.ContentType = value; }
        public bool SuppressEntityBody { get => Context.SuppressEntityBody; set => Context.SuppressEntityBody = value; }
        public string StatusDescription { get => Context.StatusDescription; set => Context.StatusDescription = value; }
        public HttpStatusCode StatusCode { get => Context.StatusCode; set => Context.StatusCode = value; }
        public string Location { get => Context.Location; set => Context.Location = value; }
        public DateTime LastModified { get => Context.LastModified; set => Context.LastModified = value; }

        public WebHeaderCollection Headers => Context.Headers;

        public string ETag { get => Context.ETag; set => Context.ETag = value; }

        public Encoding BindingWriteEncoding => Context.BindingWriteEncoding;

        public long ContentLength { get => Context.ContentLength; set => Context.ContentLength = value; }
        public WebMessageFormat? Format { get => Context.Format; set => Context.Format = value; }

        public void SetETag(Guid entityTag) => Context.SetETag(entityTag);

        public void SetETag(long entityTag) => Context.SetETag(entityTag);

        public void SetETag(int entityTag) => Context.SetETag(entityTag);

        public void SetETag(string entityTag) => Context.SetETag(entityTag);

        public void SetStatusAsCreated(Uri locationUri) => Context.SetStatusAsCreated(locationUri);

        public void SetStatusAsNotFound(string description) => Context.SetStatusAsNotFound(description);

        public void SetStatusAsNotFound() => Context.SetStatusAsNotFound();
    }
}
