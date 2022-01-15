using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IRestHandlerProviderReadOnly<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        IGetAllHandler<TEntity, TInterface, TId> GetAllHandler { get; }
        IGetByEntityIdentifiers<TEntity, TInterface, TId> GetByEntityIdentifiersHandler { get; }
        IGetByIdHandler<TEntity, TInterface, TId> GetByIdHandler { get; }
        IGetByIdsHandler<TEntity, TInterface, TId> GetByIdsHandler { get; }
        IGetByPropertyValuesHandler<TEntity, TInterface, TId> GetByPropertyValuesHandler { get; }
        IGetCountHandler<TEntity, TInterface, TId> GetCountHandler { get; }
        IGetMetadataHandler GetMetadataHandler { get; }
        IGetPropertyHandler<TEntity, TInterface, TId> GetPropertyHandler { get; }
    }
}