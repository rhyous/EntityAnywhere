using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    [ServiceContract]
    public interface IGenerateRepositoryAsync
    {
        /// <summary>
        /// Calls Generate on every entity that supports entity generation.
        /// </summary>
        /// <returns>List<RepositoryGenerationResult></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Generate", ResponseFormat = WebMessageFormat.Json)]
        Task<List<RepositoryGenerationResult>> GenerateAsync();

        /// <summary>
        /// Calls InsertSeedData on every entity that has seed data.
        /// </summary>
        /// <returns>List<RepositorySeedResult></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$Seed", ResponseFormat = WebMessageFormat.Json)]
        Task<List<RepositorySeedResult>> InsertSeedDataAsync();

        /// <summary>
        /// Builds default Entity, EntityProperty, and EntityGroup settings
        /// </summary>
        /// <returns>List<RepositorySeedResult></returns>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "$EntitySettings", ResponseFormat = WebMessageFormat.Json)]
        Task<Dictionary<string, EntitySetting>> BuildEntitySettings();
    }
}
