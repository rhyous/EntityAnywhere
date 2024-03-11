using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Mime;

namespace Rhyous.Wrappers
{
    public interface IIncomingWebRequestContext
    {
        string Accept { get; }
        string Method { get; }
        WebHeaderCollection Headers { get; }
        DateTime? IfUnmodifiedSince { get; }
        DateTime? IfModifiedSince { get; }
        IEnumerable<string> IfNoneMatch { get; }
        IEnumerable<string> IfMatch { get; }
        string ContentType { get; }
        long ContentLength { get; }
        UriTemplateMatch UriTemplateMatch { get; set; }
        string UserAgent { get; }
        void CheckConditionalRetrieve(DateTime lastModified);
        void CheckConditionalRetrieve(Guid entityTag);
        void CheckConditionalRetrieve(long entityTag);
        void CheckConditionalRetrieve(int entityTag);
        void CheckConditionalRetrieve(string entityTag);
        void CheckConditionalUpdate(Guid entityTag);
        void CheckConditionalUpdate(long entityTag);
        void CheckConditionalUpdate(int entityTag);
        void CheckConditionalUpdate(string entityTag);
        Collection<ContentType> GetAcceptHeaderElements();
    }
}