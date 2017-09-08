using LinqKit;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// A custom Addendum web service that adds a method to get Addenda by an EntityIdentifier, which includes Entity name and the Entity's id.
    /// </summary>
    [CustomWebService("AddendumWebService", typeof(IAddendumWebService), typeof(Addendum))]
    public class AddendumWebService : EntityWebService<Addendum, IAddendum, long, ServiceCommon<Addendum, IAddendum, long>>, IAddendumWebService
    {
        /// <inheritdoc />
        public List<OdataObject<Addendum>> GetByEntityIdentifiers(List<EntityIdentifier> entityIdentifiers)
        {
            var expression = PredicateBuilder.New<Addendum>();
            foreach (var identifier in entityIdentifiers)
            {
                expression.Or(e => e.Entity == identifier.Entity && e.EntityId == identifier.EntityId);
            }
            return Service.Get(expression).ToConcrete<Addendum>().ToList().AsOdata(RequestUri);
        }
    }
}
