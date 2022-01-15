using System;
using System.Net;
using System.ServiceModel.Web;

namespace Rhyous.Wrappers
{
    public class IncomingWebResponseContextWrapper : IIncomingWebResponseContext
    {
        public IncomingWebResponseContext Context;

        public IncomingWebResponseContextWrapper(IncomingWebResponseContext context)
        {
            Context = context ?? throw new ArgumentNullException("context");
        }

        public long ContentLength => Context.ContentLength;

        public string ContentType => Context.ContentType;

        public string ETag => Context.ETag;

        public WebHeaderCollection Headers => Context.Headers;

        public string Location => Context.Location;

        public HttpStatusCode StatusCode => Context.StatusCode;

        public string StatusDescription => Context.StatusDescription;
    }
}
