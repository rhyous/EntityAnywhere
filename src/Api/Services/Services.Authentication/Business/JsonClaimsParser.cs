using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public class JsonClaimsParser
    {
        public static List<string> Parse(IClaimConfiguration claimConfiguration, RelatedEntityCollection relatedEntityCollection)
        {
            var relatedEntity = claimConfiguration.Entity;
            var relatedEntityProperty = claimConfiguration.EntityProperty;
            var list = new List<string>();
            foreach (var re in relatedEntityCollection)
            {
                var jsonObj = JObject.Parse(re.Object.ToString());
                var value = jsonObj[relatedEntityProperty].ToString();
                list.Add(value);
            }
            return list;
        }
    }
}