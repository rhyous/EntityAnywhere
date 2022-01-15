using Rhyous.Odata;
using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// A service contract for an Entity Extension. This inherits the service contract for a regular entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TId">The entity id type. However, because TEntity must implement IExtension entity, TId will always be a long.</typeparam>
    [ServiceContract]
    public interface IExtensionEntityWebService<TEntity> : IEntityWebService<TEntity, long>
        where TEntity : class, IExtensionEntity, new()
    {
        /// <summary>
        /// Gets a list of Entity Extensions by a list of EntityIdentifiers.
        /// </summary>
        /// <param name="entityIdentifiers">The list of entity identifiers, which includes Entity name and the Entity's id.</param>
        /// <returns>A list of Entity Extensions.</returns>
        /// <remarks>You can get Entity Extensions for multiple entities with this call.</remarks>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        OdataObjectCollection<TEntity, long> GetByEntityIdentifiers(List<EntityIdentifier> EntityIdentifiers);
        /// <summary>
        /// Gets a list of Entity Extensions by a list of EntityIds.
        /// </summary>
        /// <param name="entityIdList">The list of entity identifiers, which includes Entity name and a list of that Entity's id.</param>
        /// <returns>A list of Entity Extensions.</returns>
        /// <remarks>You can get Entity Extensions for only one entity with this call.</remarks>
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        OdataObjectCollection<TEntity, long> GetByEntityIds(string entity, List<string> ids);
    }
}