using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// This is a root service for root actions such as:
    /// 1. Generating Metadata for all services.
    /// 2. Seeding all entities.
    /// 3. Generating the repository for all entities.
    /// </summary>
    [CustomWebService("RootWebService", typeof(IRootWebService), null, "Service")]
    public class RootWebService : IRootWebService
    {
        private readonly IRootHandlerProvider _RootHandlerProvider;
        private readonly IEntityList _EntityList;

        public RootWebService(IRootHandlerProvider rootHandlerProvider,
                           IEntityList entityList)
        {
            _RootHandlerProvider = rootHandlerProvider;
            _EntityList = entityList;
        }

        /// <inheritdoc />
        public async Task<CsdlDocument> GetMetadataAsync()
               => await _RootHandlerProvider.GetMetadataHandler.Handle(_EntityList.Entities);

        /// <inheritdoc />
        public async Task<List<RepositoryGenerationResult>> GenerateAsync() 
               => await _RootHandlerProvider.GenerateHandler.Handle();

        public async Task<List<RepositorySeedResult>> InsertSeedDataAsync() 
               => await _RootHandlerProvider.SeedEntityHandler.Handle();

        public async Task<Dictionary<string, EntitySetting>> BuildEntitySettings() 
               => await _RootHandlerProvider.EntitySettingsHandler.Handle();

        public void Dispose()
        {
        }
    }
}