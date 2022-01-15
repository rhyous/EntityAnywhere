using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IServiceHandlerProvider<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        IAddHandler<TEntity, TInterface, TId> AddHandler { get; }
        IDeleteHandler<TEntity, TInterface, TId> DeleteHandler { get; }
        IGenerateRepositoryHandler<TEntity, TInterface, TId> GenerateRepositoryHandler { get; }
        IInsertSeedDataHandler<TEntity, TInterface, TId> InsertSeedDataHandler { get; }
        IQueryableHandler<TEntity, TInterface, TId> QueryableHandler { get; }
        IGetByIdHandler<TEntity, TInterface, TId> GetByIdHandler { get; }
        IGetByIdsHandler<TEntity, TInterface, TId> GetByIdsHandler { get; }
        IGetPropertyValueHandler<TEntity, TInterface, TId> GetPropertyValueHandler { get; }
        IGetByPropertyValuesHandler<TEntity, TInterface, TId> GetByPropertyValuesHandler { get; }
        IUpdateHandler<TEntity, TInterface, TId> UpdateHandler { get; }
    }
}