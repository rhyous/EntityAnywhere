using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IServiceHandlerProviderAltKey<TEntity, TInterface, TId, TAltKey> : IServiceHandlerProvider<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        IAddAltKeyHandler<TEntity, TInterface, TId, TAltKey> AddAltKeyHandler { get; }
        IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> GetByAlternateKeyHandler { get; }
        ISearchByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> SearchByAlternateKeyHandler { get; }
        IUpdateAltKeyHandler<TEntity, TInterface, TId, TAltKey> UpdateAltKeyHandler { get; }
    }
}
