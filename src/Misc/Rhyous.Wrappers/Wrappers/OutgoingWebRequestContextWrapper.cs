using System;
using System.Net;
using System.ServiceModel.Web;

namespace Rhyous.Wrappers
{
    public class OutgoingWebRequestContextWrapper : IOutgoingWebRequestContext
    {
        public OutgoingWebRequestContext Context;

        public OutgoingWebRequestContextWrapper(OutgoingWebRequestContext context)
        {
            Context = context ?? throw new ArgumentNullException("context");
        }

        public string Accept { get =>  Context.Accept; set =>  Context.Accept = value; }
        public long ContentLength { get =>  Context.ContentLength; set =>  Context.ContentLength = value; }
        public string ContentType { get =>  Context.ContentType; set =>  Context.ContentType = value; }

        public WebHeaderCollection Headers =>  Context.Headers;

        public string IfMatch { get =>  Context.IfMatch; set => Context.IfMatch = value; }
        public string IfModifiedSince { get =>  Context.IfModifiedSince; set => Context.IfModifiedSince = value; }
        public string IfNoneMatch { get =>  Context.IfNoneMatch; set => Context.IfNoneMatch = value; }
        public string IfUnmodifiedSince { get =>  Context.IfUnmodifiedSince; set => Context.IfUnmodifiedSince = value; }
        public string Method { get =>  Context.Method; set => Context.Method = value; }
        public bool SuppressEntityBody { get =>  Context.SuppressEntityBody; set => Context.SuppressEntityBody = value; }
        public string UserAgent { get =>  Context.UserAgent; set => Context.UserAgent = value; }
    }
}
