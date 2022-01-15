using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Clients2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public class EntityMetadataAttribute : Attribute, ITestDataSource
    {
        private readonly HashSet<string> _ExcludedEntities;
        private readonly string ConfigSourceName;
        private CsdlDocument _CsdlDocument;
        private CsdlDocumentProvider _CsdlDocumentProvider;

        public EntityMetadataAttribute(string configSourceName, params string[] exclude)
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
                yield return new object[] { new KeyValuePair<string, CsdlEntity>(kvp.Key, kvp.Value as CsdlEntity) };
            }
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            return ((KeyValuePair<string, CsdlEntity>)data[0]).Key;
        }
    }
}