using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Rhyous.Wrappers
{
    public class HttpResponseWrapper : IHttpResponse
    {
        private readonly HttpResponse _HttpResponse;

        public HttpResponseWrapper(HttpResponse httpResponse)
        {
            _HttpResponse = httpResponse;
        }

        public HttpContext HttpContext => _HttpResponse.HttpContext;

        public int StatusCode { get => _HttpResponse.StatusCode; set => _HttpResponse.StatusCode = value; }

        public IHeaderDictionary Headers => _HttpResponse.Headers;

        public Stream Body { get => _HttpResponse.Body; set => _HttpResponse.Body = value; }
        public long? ContentLength { get => _HttpResponse.ContentLength; set => _HttpResponse.ContentLength = value; }
        public string ContentType { get => _HttpResponse.ContentType; set => _HttpResponse.ContentType = value; }

        public IResponseCookies Cookies => _HttpResponse.Cookies;

        public bool HasStarted => _HttpResponse.HasStarted;

        public void OnCompleted(Func<object, Task> callback, object state) => _HttpResponse.OnCompleted(callback, state);

        public void OnCompleted(Func<Task> callback) => _HttpResponse.OnCompleted(callback);

        public void OnStarting(Func<object, Task> callback, object state) => _HttpResponse.OnStarting(callback, state);

        public void OnStarting(Func<Task> callback) => _HttpResponse.OnStarting(callback);

        public void Redirect(string location) => _HttpResponse.Redirect(location);

        public void Redirect(string location, bool permanent) => _HttpResponse.Redirect(location, permanent);

        public void RegisterForDispose(IDisposable disposable) => _HttpResponse.RegisterForDispose(disposable);
    }
}