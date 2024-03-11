using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// This is a root service for root actions such as:
    /// 1. Generating Metadata for all services.
    /// 2. Seeding all entities.
    /// 3. Generating the repository for all entities.
    /// </summary>
    [Route("api/Service")]
    public class RootWebService : ControllerBase, IRootWebService
    {
        private readonly IRestHandlerProvider _RestHandlerProvider;

        public RootWebService(IRestHandlerProvider restHandlerProvider)
        {
            _RestHandlerProvider = restHandlerProvider;
        }

        /// <inheritdoc />
        [AllowAnonymous]
        [HttpGet("$Metadata")]
        public async Task<CsdlDocument> GetMetadataAsync()
               => await _RestHandlerProvider.Provide<IGetMetadataHandler>().Handle();

        /// <inheritdoc />
        [HttpGet("$Generate")]
        public async Task<List<RepositoryGenerationResult>> GenerateAsync() 
               => await _RestHandlerProvider.Provide<IGenerateHandler>().Handle();

        [HttpGet("$Seed")]
        public async Task<List<RepositorySeedResult>> InsertSeedDataAsync() 
               => await _RestHandlerProvider.Provide<ISeedEntityHandler>().Handle();

        [HttpGet("$EntitySettings")]
        public async Task<EntitySettingsDictionaryDto> BuildEntitySettings() 
               => await _RestHandlerProvider.Provide<IEntitySettingsHandler>().Handle();

        [HttpGet("$Impersonate/{roleId}")]
        public async Task<Token> ImpersonateAsync(string roleId)
               => await _RestHandlerProvider.Provide<IImpersonationHandler>().HandleAsync(roleId.To<int>());

        public void Dispose()
        {
        }
    }
}