using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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
        /// Returns the HttpClient this EntityClient will use.
        /// </summary>
        HttpClient HttpClient { get; }

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
        List<OdataObject<TEntity, TId>> GetByQueryParameters(string queryParameters);

        /// <summary>
        /// Gets all entities with the provided ids.
        /// </summary>
        /// <param name="ids">A list of entity ids.</param>
        /// <returns>All entities with the provided ids.</returns>
        List<OdataObject<TEntity, TId>> GetByIds(IEnumerable<TId> ids);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService.svc/.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        List<OdataObject<TEntity, TId>> GetByCustomUrl(string urlPart);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <remarks>This overload specifically is used for HttpClient actions other than GET, such as POST, that has content.</remarks>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService.svc/.</param>
        /// <param name="httpMethod"></param>
        /// <param name="content">The HttpContent form. It must already be in the correct format.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        List<OdataObject<TEntity, TId>> GetByCustomUrl(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, HttpContent content);

        /// <summary>
        /// This method allows for this common entity client to work with custom entities. A custom web service path can be called with this method.
        /// </summary>
        /// <remarks>This overload specifically is used for HttpClient actions other than GET, such as POST, that has content.</remarks>
        /// <param name="urlPart">The url part to the right of the service. Include only the part of the url after the https://hostname/path/EntityService.svc/ part.</param>
        /// <param name="httpMethod">The HttpClient method to call.</param>
        /// <param name="content">The content in object form, it will be converted to JSON.</param>
        /// <returns>A list of entities returned by the custom service.</returns>
        List<OdataObject<TEntity, TId>> GetByCustomUrl(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, object content);
    }
}
