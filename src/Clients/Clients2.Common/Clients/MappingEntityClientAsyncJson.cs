using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{

    public class MappingEntityClientAsync : EntityClientAsync, IMappingEntityClientAsync
    {
        private readonly IMappingEntitySettings _MappingEntitySettings;

        public MappingEntityClientAsync(IEntityClientConnectionSettings entityClientSettings,
                                        IMappingEntitySettings mappingEntitySettings,
                                        IHttpClientRunner httpClientRunner)
            : base(entityClientSettings, httpClientRunner)
        {
            _MappingEntitySettings = mappingEntitySettings ?? throw new ArgumentNullException(nameof(mappingEntitySettings));
        }

        /// <inheritdoc />
        public async Task<string> GetByE1IdsAsync(IEnumerable<string> ids, bool forwardExceptions = true)
        {
            return await GetByMappedEntityAsync(_MappingEntitySettings.Entity1Pluralized, ids);
        }

        /// <inheritdoc />
        public async Task<string> GetByE1IdsAsync(IEnumerable<string> ids, string urlParameters, bool forwardExceptions = true)
        {
            return await GetByMappedEntityAsync(_MappingEntitySettings.Entity1Pluralized, ids, urlParameters);
        }

        /// <inheritdoc />
        public async Task<string> GetByE2IdsAsync(IEnumerable<string> ids, bool forwardExceptions = true)
        {
            return await GetByMappedEntityAsync(_MappingEntitySettings.Entity2Pluralized, ids);
        }

        /// <inheritdoc />
        public async Task<string> GetByE2IdsAsync(IEnumerable<string> ids, string urlParameters, bool forwardExceptions = true)
        {
            return await GetByMappedEntityAsync(_MappingEntitySettings.Entity2Pluralized, ids, urlParameters);
        }

        /// <inheritdoc />
        internal async Task<string> GetByMappedEntityAsync(string pluralizedEntityName, IEnumerable<string> ids, string urlParameters = null, bool forwardExceptions = true)
        {
            if (ids == null || !ids.Any()) { throw new ArgumentException(nameof(ids)); }
            var url = $"{ServiceUrl}/{EntityPluralized}/{pluralizedEntityName}/Ids";
            url = AppendUrlParameters(urlParameters, url);
            return await _HttpClientRunner.RunAndDeserialize<List<string>, string>(HttpMethod.Post, url, ids.ToList(), _EntityClientConnectionSettings.JsonSerializerSettings, forwardExceptions);
        }
    }
}