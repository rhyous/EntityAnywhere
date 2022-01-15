using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Clients2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public class PostEntityMetadataAttribute : Attribute, ITestDataSource
    {
        private readonly HashSet<string> _ExcludedEntities;
        private CsdlDocument _CsdlDocument;
        private CsdlDocumentProvider _CsdlDocumentProvider;
        private readonly string ConfigSourceName;

        public PostEntityMetadataAttribute(string configSourceName, params string[] exclude)
        {
            _ExcludedEntities = new HashSet<string>(exclude, StringComparer.OrdinalIgnoreCase);
            ConfigSourceName = configSourceName;
        }

        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            var config = methodInfo.DeclaringType.GetTypeInfo().GetDeclaredField(ConfigSourceName).GetValue(null) as IEntityClientConfig;

            _CsdlDocumentProvider = new CsdlDocumentProvider(config);

            if (_CsdlDocument == null)
                _CsdlDocument = _CsdlDocumentProvider.Provide();
            var entities = (_CsdlDocument.Schemas.First().Value as CsdlSchema).Entities;
            foreach (var kvp in entities)
            {
                if (_ExcludedEntities != null && _ExcludedEntities.Contains(kvp.Key, StringComparer.OrdinalIgnoreCase))
                    continue;
                var csdlEntity = kvp.Value as CsdlEntity;
                var prereqs = new List<KeyValuePair<string, CsdlEntity>>();
                // Get dependencies recursively
                var relatedEntities = GetRelatedEntities(kvp.Key, csdlEntity, entities);
                if (relatedEntities.Any())
                    prereqs.AddRange(relatedEntities);
                // Add actual entity last
                var testObject = new KeyValuePair<string, CsdlEntity>(kvp.Key, kvp.Value as CsdlEntity);
                yield return new object[] { testObject, prereqs };
            }
        }

        private static IEnumerable<KeyValuePair<string, CsdlEntity>> GetRelatedEntities(string entityName, CsdlEntity csdlEntity, Dictionary<string, object> entities)
        {
            foreach (var kvp in csdlEntity.Properties.Where(x => !x.Key.StartsWith("$") || !x.Key.StartsWith("@")))
            {
                if (kvp.Value is JToken jtoken)
                {
                    var relatedEntity = jtoken.Value<string>("$NavigationKey");
                    if (string.IsNullOrWhiteSpace(relatedEntity))
                        continue;
                    var reKvp = entities.FirstOrDefault(e => e.Key == relatedEntity);
                    var relatedEntities = GetRelatedEntities(reKvp.Key, reKvp.Value as CsdlEntity, entities);
                    if (relatedEntities != null && relatedEntity.Any())
                    {
                        foreach (var re in relatedEntities)
                        {
                            if (re.Key == entityName)
                                throw new CircularEntityDependecyException(entityName);
                            yield return re;
                        }
                    }
                    yield return new KeyValuePair<string, CsdlEntity>(reKvp.Key, reKvp.Value as CsdlEntity);
                }
            }
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return ((KeyValuePair<string, CsdlEntity>)data[0]).Key;
        }
    }
}
