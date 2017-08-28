using Rhyous.WebFramework.Interfaces;
using System;
using System.Configuration;
using System.ServiceModel.Web;
using System.Web;

namespace Rhyous.WebFramework.WebServices
{
    public class HttpContextProvider : IHttpContextProvider
    {
        public HttpContext CurrentHttpContext => HttpContext.Current;
        public WebOperationContext CurrentWebOperationContext => WebOperationContext.Current;

        public string WebHost => _WebHost ?? (_WebHost = BuildWebHost(Uri));
        private string _WebHost;

        public Uri Uri => CurrentHttpContext?.Request?.Url ?? CurrentWebOperationContext?.IncomingRequest?.UriTemplateMatch?.RequestUri;

        internal string BuildWebHost(Uri uri)
        {
            if (uri == null)
                return ConfigurationManager.AppSettings.Get("WebHost", string.Empty);
            var url = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
            if ((uri.Scheme.Equals("http") && uri.Port != 80) || uri.Scheme.Equals("https") && uri.Port != 443)
                url += ":" + uri.Port;
            return url;
        }
    }
}