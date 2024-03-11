using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyous.Wrappers
{
    public class HttpRequestWrapper : IHttpRequest
    {
        public readonly HttpRequest _HttpRequest;

        public HttpRequestWrapper(HttpRequest httpRequest)
        {
            _HttpRequest = httpRequest;
        }

        public HttpContext HttpContext => _HttpRequest.HttpContext;

        public string Method
        {
            get => _HttpRequest.Method;
            set => _HttpRequest.Method = value;
        }
        public string Scheme
        {
            get => _HttpRequest.Scheme;
            set => _HttpRequest.Scheme = value;
        }
        public bool IsHttps
        {
            get => _HttpRequest.IsHttps;
            set => _HttpRequest.IsHttps = value;
        }
        public HostString Host
        {
            get => _HttpRequest.Host;
            set => _HttpRequest.Host = value;
        }
        public PathString PathBase
        {
            get => _HttpRequest.PathBase;
            set => _HttpRequest.PathBase = value;
        }
        public PathString Path
        {
            get => _HttpRequest.Path;
            set => _HttpRequest.Path = value;
        }
        public QueryString QueryString
        {
            get => _HttpRequest.QueryString;
            set => _HttpRequest.QueryString = value;
        }
        public IQueryCollection Query
        {
            get => _HttpRequest.Query;
            set => _HttpRequest.Query = value;
        }
        public string Protocol
        {
            get => _HttpRequest.Protocol;
            set => _HttpRequest.Protocol = value;
        }

        public IHeaderDictionary Headers => _HttpRequest.Headers;

        public IRequestCookieCollection Cookies
        {
            get => _HttpRequest.Cookies;
            set => _HttpRequest.Cookies = value;
        }
        public long? ContentLength
        {
            get => _HttpRequest.ContentLength;
            set => _HttpRequest.ContentLength = value;
        }
        public string ContentType
        {
            get => _HttpRequest.ContentType;
            set => _HttpRequest.ContentType = value;
        }
        public Stream Body
        {
            get => _HttpRequest.Body;
            set => _HttpRequest.Body = value;
        }

        public bool HasFormContentType => _HttpRequest.HasFormContentType;

        public IFormCollection Form
        {
            get => _HttpRequest.Form;
            set => _HttpRequest.Form = value;
        }

        public Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
            => _HttpRequest.ReadFormAsync(cancellationToken);

        public string GetDisplayUrl() => _HttpRequest.GetDisplayUrl();
    }
}