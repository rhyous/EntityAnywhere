using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public class AccessController : IAccessController
    {
        public static readonly string AllowAnonymousSvcPages = "AllowAnonymousSvcPages";
        public static readonly string AllowAnonymousSvcHelpPages = "AllowAnonymousSvcHelpPages";
        private readonly IAppSettings _AppSettings;
        private readonly IAnonymousAllowedUrls _AnonymousAllowedUrls;

        public AccessController(IAppSettings appSettings,
                                IAnonymousAllowedUrls anonymousAllowedUrls)
        {
            _AppSettings = appSettings;
            _AnonymousAllowedUrls = anonymousAllowedUrls;
        }

        public bool IsAnonymousAllowed(string absolutePath)
        {
            if (absolutePath.Contains("/API", StringComparison.OrdinalIgnoreCase))
            {
                absolutePath = absolutePath.Substring(4);
            }

            var allowAnonymousSvcPages = _AppSettings.Collection.Get(AllowAnonymousSvcPages, true);
            var isAnonymousCall = _AnonymousAllowedUrls.Urls.Contains(absolutePath);
            return allowAnonymousSvcPages && (absolutePath.EndsWith(".svc") || absolutePath.Contains("/help") || isAnonymousCall);
        }

        public bool IsSystemAdmin(NameValueCollection headers, IWebOperationContext webContext)
        {
            var entityAdminToken = _AppSettings.Collection.Get("EntityAdminToken", "");
            if (!string.IsNullOrWhiteSpace(entityAdminToken) && headers.Get("EntityAdminToken") == entityAdminToken)
            {
                webContext.IncomingRequest.Headers.Add("UserId", "1");
                return true; // 1 should always be the Admin user
            }
            return false;
        }   
    }
}