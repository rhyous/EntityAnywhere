using Newtonsoft.Json;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class MappingEntityClientAsync : EntityClientAsync, IMappingEntityClientAsync
    {
        #region Constructors
        public MappingEntityClientAsync(string entity, string entity1, string entity2, string entity1Property = null, string entity2property = null)
            : base (entity)
        {
            _Entity1 = entity1;
            _Entity2 = entity2;
        }

        public MappingEntityClientAsync(string entity, string entity1, string entity2, bool useMicrosoftDateFormat, string entity1Property = null, string entity2property = null)
            : base(entity, useMicrosoftDateFormat)
        {
            _Entity1 = entity1;
            _Entity2 = entity2;
        }

        public MappingEntityClientAsync(string entity, string entity1, string entity2, JsonSerializerSettings jsonSerializerSettings, string entity1Property = null, string entity2property = null) 
            : base(entity, jsonSerializerSettings)
        {
            _Entity1 = entity1;
            _Entity2 = entity2;
        }
        #endregion

        /// <inheritdoc />
        public string Entity1 => _Entity1;
        private string _Entity1;

        /// <inheritdoc />
        public string Entity1Pluralized { get { return _Entity1Pluralized ?? (_Entity1Pluralized = PluralizationDictionary.Instance.GetValueOrDefault(Entity1)); } }
        private string _Entity1Pluralized;

        /// <inheritdoc />
        public string Entity1Property { get { return _Entity1Property ?? (_Entity1Property = $"{Entity1}Id"); } }
        private string _Entity1Property;

        /// <inheritdoc />
        public string Entity2 => _Entity2;
        private string _Entity2;

        /// <inheritdoc />
        public string Entity2Pluralized { get { return _Entity2Pluralized ?? (_Entity2Pluralized = PluralizationDictionary.Instance.GetValueOrDefault(Entity2)); } }
        private string _Entity2Pluralized;

        /// <inheritdoc />
        public string Entity2Property { get { return _Entity2Property ?? (_Entity2Property = $"{Entity2}Id"); } }
        private string _Entity2Property;

        /// <inheritdoc />
        public virtual async Task<string> GetByE1IdsAsync(IEnumerable<string> ids, string urlParameters = null)
        {
            return await GetByMappedEntityAsync(Entity1Pluralized, ids.ToList(), urlParameters);
        }

        /// <inheritdoc />
        public virtual async Task<string> GetByE2IdsAsync(IEnumerable<string> ids, string urlParameters = null)
        {
            return await GetByMappedEntityAsync(Entity2Pluralized, ids.ToList(), urlParameters);
        }

        /// <inheritdoc />
        internal virtual async Task<string> GetByMappedEntityAsync(string pluralizedEntityName, List<string> ids, string urlParameters = null)
        {
            var url = $"{ServiceUrl}/{EntityPluralized}/{pluralizedEntityName}/Ids";
            url = AppendUrlParameters(urlParameters, url);
            return await HttpClientRunner.RunAndDeserialize<List<string>, string>(HttpClient.PostAsync, url, ids);
        }
    }
}