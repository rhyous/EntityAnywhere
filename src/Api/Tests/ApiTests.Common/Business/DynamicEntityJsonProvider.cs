using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public class DynamicEntityJsonProvider
    {
        private readonly HashSet<string> IgnoreProperties = new HashSet<string>
        {
            "Id",
            "CreateDate",
            "CreatedBy",
            "LastUpdated",
            "LastUpdatedBy"
        };
        private readonly EntityTypeJsonProviderDictionary _EntityTypeJsonProviderDictionary;

        public DynamicEntityJsonProvider(EntityTypeJsonProviderDictionary entityTypeJsonProviderDictionary)
        {
            _EntityTypeJsonProviderDictionary = entityTypeJsonProviderDictionary 
                                             ?? throw new ArgumentNullException(nameof(entityTypeJsonProviderDictionary));
        }

        public string Provide(string entity, CsdlEntity entityMetadata, IDictionary<string, OdataObject> postedPrereqs)
        {
            var testEntityObject = new JObject();
            var propertiesNeedValues = entityMetadata.Properties
                                                     .Where(kvp => !kvp.Key.StartsWith("@") // Ignore attributes
                                                                && !IgnoreProperties.Contains(kvp.Key)); // Ignore ignored properties
            foreach (var kvp in propertiesNeedValues)
            {
                var propertyName = kvp.Key;
                var csdlProp = kvp.Value as JObject;
                // Ignore NavigationProperties
                if (csdlProp.TryGetValue("$Kind", out JToken kind) && kind.ToString() == "NavigationProperty")
                    continue;
                var edmType = (string)csdlProp["$Type"];
                if (string.IsNullOrWhiteSpace(edmType))
                    edmType = (string)csdlProp["$UnderlyingType"];
                // If a $NavigationKey 
                if (csdlProp.TryGetValue("$NavigationKey", out JToken navkey))
                {
                    var relatedEntity = (string)navkey;
                    if (!postedPrereqs.TryGetValue(relatedEntity, out OdataObject prereq))
                        throw new PrerequisiteEntityMissingException(relatedEntity);
                    testEntityObject.Add(propertyName, prereq.Id); // What if it isn't Id but an alternate key?
                    continue;
                }
                var data = _EntityTypeJsonProviderDictionary[edmType].Invoke(propertyName, csdlProp);
                testEntityObject.Add(propertyName, data);
            }
            return $"[{testEntityObject}]";
        }
    }
}