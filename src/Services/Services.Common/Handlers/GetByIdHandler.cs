using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    class GetByIdHandler<TEntity, TInterface, TId> : IGetByIdHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;

        public GetByIdHandler(IRepository<TEntity, TInterface, TId> repository)
        {
            _Repository = repository;         
        }

        public TInterface Get(TId Id)
        {
            return _Repository.Get(Id);
        }
    }
}