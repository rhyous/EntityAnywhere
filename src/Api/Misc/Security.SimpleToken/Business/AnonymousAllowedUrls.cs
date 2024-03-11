using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Security
{
    public class AnonymousAllowedUrls : IAnonymousAllowedUrls
    {
        public HashSet<string> Urls => _Urls ?? (_Urls = GetUrls());
        private HashSet<string> _Urls;

        private HashSet<string> GetUrls()
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
            };
        }
    }
}