using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// This is the default service for an entity that has the AlternateKeyAttribute of a string, such as Name or Type or something. 
    /// It currently must be a string.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public interface IServiceCommonAlternateKey<TEntity, TInterface, TId> : IServiceCommon<TEntity,TInterface, TId>
        where TInterface : IEntity<TId>
        where TEntity : class, TInterface
    {
        /// <summary>
        /// This method searches by name. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<TInterface> Search(string name);

        /// <summary>
        /// The expression used to get the value of the alternate key property: x => x.Name
        /// </summary>
        Expression<Func<TEntity, string>> PropertyExpression { get; }


        TInterface Get(string id);
    }
}
