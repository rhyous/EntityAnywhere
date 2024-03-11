using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGenerateRepositoryAsync
    {
        /// <summary>
        /// Calls Generate on every entity that supports entity generation.
        /// </summary>
        /// <returns>List<RepositoryGenerationResult></returns>
        Task<List<RepositoryGenerationResult>> GenerateAsync();

        /// <summary>
        /// Calls InsertSeedData on every entity that has seed data.
        /// </summary>
        /// <returns>List<RepositorySeedResult></returns>
        Task<List<RepositorySeedResult>> InsertSeedDataAsync();

        /// <summary>
        /// Builds default Entity, EntityProperty, and EntityGroup settings
        /// </summary>
        /// <returns>List<RepositorySeedResult></returns>
        Task<EntitySettingsDictionaryDto> BuildEntitySettings();
    }
}