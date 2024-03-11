using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class RootClient : IRootClient
    {
        private readonly IHttpClientRunnerNoHeaders _HttpClientRunner;
        private readonly IEntityClientConfig _Config;

        public RootClient(IHttpClientRunnerNoHeaders httpClientRunner,
                              IEntityClientConfig entityClientConfig)
        {
            _HttpClientRunner = httpClientRunner;
            _Config = entityClientConfig;
        }
        public virtual string ServiceUrl => string.IsNullOrEmpty(_Config.EntitySubpath) ? StringConcat.WithSeparator('/', _Config.EntityWebHost) : StringConcat.WithSeparator('/', _Config.EntityWebHost, _Config.EntitySubpath);

        public async Task<CsdlDocument> GetMetadataAsync(bool forceUpdate = false, bool forwardExceptions = true)
            => await SendAsync<CsdlDocument>($"{ServiceUrl}/Service/$Metadata", forceUpdate, forwardExceptions);

        public async Task<EntitySettingsDictionary> ConfigureEntitySettingsAsync(bool forceUpdate = false, bool forwardExceptions = true)
            => await SendAsync<EntitySettingsDictionary>($"{ServiceUrl}/Service/$EntitySettings", forceUpdate, forwardExceptions);

        public async Task<List<RepositoryGenerationResult>> GenerateRepository(bool forwardExceptions = true)
            => await SendAsync<List<RepositoryGenerationResult>>($"{ServiceUrl}/Service/$Generate", false, forwardExceptions);

        public async Task<List<RepositorySeedResult>> InsertSeedDataAsync(bool forwardExceptions = true)
            => await SendAsync<List<RepositorySeedResult>>($"{ServiceUrl}/Service/$Seed", false, forwardExceptions);

        private async Task<T> SendAsync<T>(string url, bool forceUpdate, bool forwardExceptions)
        {
            if (forceUpdate)
                url += "?ForceUpdate=true";
            return await _HttpClientRunner.RunAndDeserialize<T>(HttpMethod.Get, url, forwardExceptions);
        }
    }
}