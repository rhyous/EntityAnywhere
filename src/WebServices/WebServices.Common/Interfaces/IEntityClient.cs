using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// A common class that any client can implement to talk to any entity.
    /// It inherites from IEntityWebService so all the same methods that exist on the common webservice are available to all client implementations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type</typeparam>
    /// <typeparam name="TId">The entity id type</typeparam>
    public interface IEntityClient<TEntity, TId> : IEntityWebService<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
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

        /// <summary>
        /// This is used to find the first part of the url: http or https and hostname, etc.
        /// https://host/path/to/api
        /// </summary>
        IHttpContextProvider HttpContextProvider { get; }

        /// <summary>
        /// This provides an additional option to make a get call with query parameters.
        /// </summary>
        /// <param name="queryParameters">a string of query parameters that you would find left of the URL. Starts with a ?.</param>
        /// <returns>A list of entities that match the query parameters.</returns>
        List<OdataObject<TEntity>> GetByQueryParameters(string queryParameters);

        /// <summary>
        /// Gets all entities with the provided ids.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        List<OdataObject<TEntity>> GetByIds(IEnumerable<TId> ids);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hsotname/path/EntityService.svc/.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        List<OdataObject<TEntity>> GetByCustomUrl(string urlPart);
    }
}
