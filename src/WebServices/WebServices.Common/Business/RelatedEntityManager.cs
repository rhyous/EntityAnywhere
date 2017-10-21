using Newtonsoft.Json.Linq;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices
{
    public class RelatedEntityManager
    {
        public IEnumerable<RelatedEntityAttribute> GetRelatedEntityAttributes(object o)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            var attributes = o.GetType()
                              .GetProperties()
                              .Select(p => p.GetCustomAttribute<RelatedEntityAttribute>(true)).Where(a => a != null);
            return attributes;
        }

        public JObject Get(object o) {
            var list = o as IEnumerable;
            if (list != null)
            {

            }
            var relatedEntityAttributes = GetRelatedEntityAttributes(o);
            foreach (var item in relatedEntityAttributes)
            {

            }
            var jo = new JObject(o);
            return jo;
        }

    }
}
