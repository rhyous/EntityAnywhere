using System.Net.Http;

namespace Rhyous.WebFramework.Clients
{
    public interface IEntityClientBase
    {
        /// <summary>
        /// This is the url to the entity service. For example: https://host/path/to/api/Entity1Service.svc
        /// </summary>
        string ServiceUrl { get; set; }

        /// <summary>
        /// The entity name.
        /// </summary>
        string Entity { get; }


        /// <summary>
        /// The entity name pluralized.
        /// </summary>
        string EntityPluralized { get; }

        HttpClient HttpClient { get; set; }
    }
}