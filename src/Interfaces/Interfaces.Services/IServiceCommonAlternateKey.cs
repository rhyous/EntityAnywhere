using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This is the default service for an entity that has the AlternateKeyAttribute of a string, such as Name or Type or something. 
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public interface IServiceCommonAlternateKey<TEntity, TInterface, TId, TAltKey> : IServiceCommon<TEntity,TInterface, TId>
        where TInterface : IBaseEntity<TId>
        where TEntity : class, TInterface
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        /// <summary>
        /// This method searches by name. 
        /// </summary>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        List<TInterface> Search(TAltKey propertyValue);

        TInterface GetByAlternateKey(TAltKey id);
    }
}
