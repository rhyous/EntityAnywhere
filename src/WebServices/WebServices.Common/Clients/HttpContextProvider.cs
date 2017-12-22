using Rhyous.WebFramework.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This class tries to find the url and web host information.
    /// If this is not used in a web service, but in a client, then this provider expects this information to be found in appSettings of the web.config.
    ///     &lt; key="WebHost" value="https://hostname:443" /&gt;
    /// </summary>
    public class HttpContextProvider : IHttpContextProvider
    {
        /// <inheritdoc />
        public HttpContext CurrentHttpContext {
            get { return _CurrentHttpContext ?? HttpContext.Current; } // Don't store current
            set { _CurrentHttpContext = value; }
        } private HttpContext _CurrentHttpContext;

        /// <inheritdoc />
        public WebOperationContext CurrentWebOperationContext
        {
            get { return _CurrentWebOperationContext ?? WebOperationContext.Current ?? FindContext(); } // Don't store current
            set { _CurrentWebOperationContext = value; }
        } internal WebOperationContext _CurrentWebOperationContext;

        /// <inheritdoc />
        public OperationContext CurrentOperationContext => OperationContext.Current;

        /// <inheritdoc />
        public string WebHost => _WebHost ?? (_WebHost = BuildWebHost(Uri));
        private string _WebHost;

        /// <inheritdoc />
        public Uri Uri => CurrentHttpContext?.Request?.Url ?? CurrentWebOperationContext?.IncomingRequest?.UriTemplateMatch?.RequestUri;

        internal WebOperationContext FindContext()
        {
            return CallContext.LogicalGetData("WebOperationContext") as WebOperationContext;
        }

        internal string BuildWebHost(Uri uri)
        {
            if (uri == null)
                return ConfigurationManager.AppSettings.Get("WebHost", string.Empty);
            var url = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
            if ((uri.Scheme.Equals("http") && uri.Port != 80) || uri.Scheme.Equals("https") && uri.Port != 443)
                url += ":" + uri.Port;
            var subDirs = uri.LocalPath.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (subDirs != null && subDirs.Any())
            {
                foreach (var dir in subDirs)
                {
                    if (dir.EndsWith(".svc"))
                        break;
                    url += $"/{dir}";
                }
            }
            return url;
        }
    }
}