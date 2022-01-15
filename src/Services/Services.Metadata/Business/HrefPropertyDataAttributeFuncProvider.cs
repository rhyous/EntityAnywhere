using Rhyous.EntityAnywhere.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services
{
    public class HrefPropertyDataAttributeFuncProvider
    {
        public HrefPropertyDataAttributeFuncProvider()
        {
            Func = GetPropertyData;
        }
        public Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> Func { get; }

        public IEnumerable<KeyValuePair<string, object>> GetPropertyData(MemberInfo mi)
        {
            var hrefAttrib = mi.GetCustomAttribute<HRefAttribute>();
            if (hrefAttrib == null)
                return null;
            return new[] { new KeyValuePair<string, object>("@StringType", "href") };
        }
    }

}
