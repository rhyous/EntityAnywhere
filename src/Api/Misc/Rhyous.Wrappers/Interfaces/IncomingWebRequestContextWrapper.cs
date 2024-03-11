using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mime;
using System.ServiceModel.Web;

namespace Rhyous.Wrappers
{
    public class IncomingWebRequestContextWrapper : IIncomingWebRequestContext
    {        
        public IncomingWebRequestContext Context
        {
            get { return _context; }
            set { _context = value; }
        } private IncomingWebRequestContext _context;

        public IncomingWebRequestContextWrapper(IncomingWebRequestContext context)
        {
            Context = context ?? throw new ArgumentNullException("context");
        }

        public string Accept => Context.Accept;

        public string Method => Context.Method;

        public WebHeaderCollection Headers => Context.Headers;

        public DateTime? IfUnmodifiedSince => Context.IfUnmodifiedSince;

        public DateTime? IfModifiedSince => Context.IfModifiedSince;

        public IEnumerable<string> IfNoneMatch => Context.IfNoneMatch;

        public IEnumerable<string> IfMatch => Context.IfMatch;

        public string ContentType => Context.ContentType;

        public long ContentLength => Context.ContentLength;

        public UriTemplateMatch UriTemplateMatch
        {
            get
            {
                return Context.UriTemplateMatch;
            }
            set
            {
                value = Context.UriTemplateMatch;
            }
        }

        public string UserAgent => Context.UserAgent;

        public void CheckConditionalRetrieve(DateTime lastModified) => Context.CheckConditionalRetrieve(lastModified);

        public void CheckConditionalRetrieve(Guid entityTag) => Context.CheckConditionalRetrieve(entityTag);

        public void CheckConditionalRetrieve(long entityTag) => Context.CheckConditionalRetrieve(entityTag);

        public void CheckConditionalRetrieve(int entityTag) => Context.CheckConditionalRetrieve(entityTag);

        public void CheckConditionalRetrieve(string entityTag) => Context.CheckConditionalRetrieve(entityTag);

        public void CheckConditionalUpdate(Guid entityTag) => Context.CheckConditionalUpdate(entityTag);

        public void CheckConditionalUpdate(long entityTag) => Context.CheckConditionalUpdate(entityTag);

        public void CheckConditionalUpdate(int entityTag) => Context.CheckConditionalUpdate(entityTag);

        public void CheckConditionalUpdate(string entityTag) => Context.CheckConditionalUpdate(entityTag);

        public Collection<ContentType> GetAcceptHeaderElements() => Context.GetAcceptHeaderElements();
    }
}
