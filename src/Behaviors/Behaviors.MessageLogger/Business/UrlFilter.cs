using Rhyous.Collections;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Rhyous.EntityAnywhere.Behaviors
{
    /// <summary>
    /// A class to read a list of Urls to log requests and posts for.
    /// Exclude the protocol and host, i.e., the https://servername/.
    /// </summary>
    public class UrlFilter : IUrlFilter
    {
        public HashSet<string> RequestUrls { get { return GetUrls("LogRequestUrls"); } }
        public HashSet<string> ResponseUrls { get { return GetUrls("LogResponseUrls"); } }

        private HashSet<string> GetUrls(string name)
        {
            var commaSeparatedUrls = AppSettings.Get(name, "All");
            if (commaSeparatedUrls == "All")
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase) { commaSeparatedUrls };
            var urlArray = commaSeparatedUrls.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return new HashSet<string>(urlArray, StringComparer.OrdinalIgnoreCase);
        }

        internal NameValueCollection AppSettings
        {
            get { return _AppSettings ?? (_AppSettings = ConfigurationManager.AppSettings); }
            set { _AppSettings = value; }
        } private NameValueCollection _AppSettings;
    }
}