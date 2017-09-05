﻿using Rhyous.WebFramework.Interfaces;
using System;
using System.Configuration;
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
        public HttpContext CurrentHttpContext => HttpContext.Current;
        /// <inheritdoc />
        public WebOperationContext CurrentWebOperationContext => WebOperationContext.Current;

        /// <inheritdoc />
        public string WebHost => _WebHost ?? (_WebHost = BuildWebHost(Uri));
        private string _WebHost;

        /// <inheritdoc />
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