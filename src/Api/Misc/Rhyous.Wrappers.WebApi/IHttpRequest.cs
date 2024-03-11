using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyous.Wrappers
{
    public interface IHttpRequest
    {
        HttpContext HttpContext { get; }

        string Method { get; set; }

        string Scheme { get; set; }

        bool IsHttps { get; set; }

        HostString Host { get; set; }

        PathString PathBase { get; set; }

        PathString Path { get; set; }

        QueryString QueryString { get; set; }

        IQueryCollection Query { get; set; }

        string Protocol { get; set; }

        IHeaderDictionary Headers { get; }

        IRequestCookieCollection Cookies { get; set; }

        long? ContentLength { get; set; }

        string ContentType { get; set; }
        Stream Body { get; set; }

        bool HasFormContentType { get; }

        IFormCollection Form { get; set; }

        Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default(CancellationToken));
        
        string GetDisplayUrl();
    }
}
