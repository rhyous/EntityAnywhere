using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// A custom Addendum web service that adds a method to get Addenda by an EntityIdentifier, which includes Entity name and the Entity's id.
    /// </summary>
    [ServiceContract]
    public interface IAddendumWebService : IEntityWebService<Addendum, long>
    {
        /// <summary>
        /// Gets a list of Addenda by a list of EntityIdentifiers.
        /// </summary>
        /// <param name="entityIdentifiers">The list of entity identifiers, which includes Entity name and the Entity's id.</param>
        /// <returns>A list of Addenda.</returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Addenda/EntityIdentifiers", ResponseFormat = WebMessageFormat.Json)]
        OdataObjectCollection<Addendum, long> GetByEntityIdentifiers(List<EntityIdentifier> EntityIdentifiers);
    }
}
