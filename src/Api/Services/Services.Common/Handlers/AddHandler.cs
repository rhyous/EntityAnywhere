using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    class AddHandler<TEntity, TInterface, TId> : IAddHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;

        public AddHandler(IRepository<TEntity, TInterface, TId> repository)
        {
            _Repository = repository;
        }

        public Task<List<TInterface>> AddAsync(IEnumerable<TInterface> entities)
        {
            return Task.FromResult(_Repository.Create(entities));
        }
    }
}