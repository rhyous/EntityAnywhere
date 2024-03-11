using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class EntityProvider<TEntity, TId> : IEntityProvider<TEntity, TId>
        where TEntity : class, IId<TId>, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IEntityClientAsync<TEntity, TId> _Client;

        public EntityProvider(IEntityClientAsync<TEntity, TId> client)
        {
            _Client = client;
        }

        public async Task<TEntity> Provide(TId id, bool throwIfNotFound = false)
        {
            var odataEntity = await _Client.GetAsync(id);

            if (throwIfNotFound && odataEntity == null)
                throw new EntityNotFoundException($"No entity of type {typeof(TEntity)} was found with id {id}");
            return odataEntity.Object;
        }
    }
}
