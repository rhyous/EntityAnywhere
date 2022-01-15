using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IRestHandlerProvider<TEntity, TInterface, TId> : IRestHandlerProviderReadOnly<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        IDeleteHandler<TEntity, TInterface, TId> DeleteHandler { get; }
        IDeleteManyHandler<TEntity, TInterface, TId> DeleteManyHandler { get; }
        IGenerateRepositoryHandler<TEntity, TInterface, TId> GenerateRepositoryHandler { get; }
        IInsertSeedDataHandler<TEntity, TInterface, TId> InsertSeedDataHandler { get; }
        IPatchHandler<TEntity, TInterface, TId> PatchHandler { get; }
        IPostHandler<TEntity, TInterface, TId> PostHandler { get; }
        IPutHandler<TEntity, TInterface, TId> PutHandler { get; }
        IUpdatePropertyHandler<TEntity, TInterface, TId> UpdatePropertyHandler { get; }
        IPostExtensionHandler<TEntity, TInterface, TId> PostExtensionHandler { get; }
        IUpdateExtensionValueHandler<TEntity, TInterface, TId> UpdateExtensionValueHandler { get; }
    }


    public interface IRestHandlerProviderAlternateKey<TEntity, TInterface, TId, TAltKey>
                     : IRestHandlerProvider<TEntity, TInterface, TId>
    where TEntity : class, TInterface, new()
    where TInterface : IBaseEntity<TId>
    where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        IGetByAlternateKeyHandler<TEntity, TInterface, TId, TAltKey> GetByAlternateKeyHandler { get; }

    }
}
