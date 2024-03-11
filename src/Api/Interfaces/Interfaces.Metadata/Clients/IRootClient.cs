using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IRootClient
    {
        string ServiceUrl { get; }

        Task<CsdlDocument> GetMetadataAsync(bool forceUpdate = false, bool forwardExceptions = true);
        Task<EntitySettingsDictionary> ConfigureEntitySettingsAsync(bool forceUpdate = false, bool forwardExceptions = true);
        Task<List<RepositoryGenerationResult>> GenerateRepository(bool forwardExceptions = true);
        Task<List<RepositorySeedResult>> InsertSeedDataAsync(bool forwardExceptions = true);
    }
}