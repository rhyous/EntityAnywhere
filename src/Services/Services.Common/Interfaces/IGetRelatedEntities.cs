using Rhyous.Odata;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.WebFramework.Services
{
    public interface IGetRelatedEntities<TInterface>
    {
        /// <summary>
        /// Gets related entities for the type TEntity.
        /// Related entities are specified on TEntity properties using the RelatedEntityAttribute.
        /// </summary>
        /// <param name="entity">The entity to get related entities for.</param>
        /// <param name="parameters"></param>
        /// <returns>Related entites as raw json strings.</returns>
        List<RelatedEntityCollection> GetRelatedEntities(TInterface entity, NameValueCollection parameters);

        /// <summary>
        /// Gets related entities for the type TEntity given the passed in list of instances.
        /// Related entities are specified on TEntity properties using the RelatedEntityAttribute.
        /// </summary>
        /// <param name="entities">A list of entities to get related entities for.</param>
        /// <param name="parameters"></param>
        /// <returns>Related entites as raw json strings.</returns>
        List<RelatedEntityCollection> GetRelatedEntities(IEnumerable<TInterface> entities, NameValueCollection parameters);

    }
}
