using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    class DeleteHandler<TEntity, TInterface, TId> : IDeleteHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;

        public DeleteHandler(IRepository<TEntity, TInterface, TId> repository)
        {
            _Repository = repository;
        }
        /// <inheritdoc />
        public bool Delete(TId id)
        {
            return _Repository.Delete(id);
        }

        /// <inheritdoc />
        public Dictionary<TId, bool> DeleteMany(IEnumerable<TId> ids)
        {
            return _Repository.DeleteMany(ids);
        }
    }
}