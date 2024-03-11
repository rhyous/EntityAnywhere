using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System.Net;

namespace Rhyous.EntityAnywhere.WebApi
{
    internal class WebApiHttpResponseSetter : IHttpStatusCodeSetter
    {
        private readonly IHttpResponse _HttpResponse;

        public WebApiHttpResponseSetter(IHttpResponse httpResponse)
        {
            _HttpResponse = httpResponse;
        }

        public HttpStatusCode StatusCode
        {
            get => (HttpStatusCode)_HttpResponse.StatusCode;
            set => _HttpResponse.StatusCode = (int)value;
        }
    }
}