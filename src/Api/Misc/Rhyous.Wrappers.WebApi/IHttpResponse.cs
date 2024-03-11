using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rhyous.Wrappers
{
    public interface IHttpResponse
    {
        HttpContext HttpContext { get; }

        int StatusCode { get; set; }

        IHeaderDictionary Headers { get; }

        Stream Body { get; set; }

        long? ContentLength { get; set; }

        string ContentType { get; set; }

        IResponseCookies Cookies { get; }
        bool HasStarted { get; }
        void OnStarting(Func<object, Task> callback, object state);
        void OnStarting(Func<Task> callback);

        void OnCompleted(Func<object, Task> callback, object state);
        void RegisterForDispose(IDisposable disposable);
        void OnCompleted(Func<Task> callback);

        void Redirect(string location);

        void Redirect(string location, bool permanent);
    }
}