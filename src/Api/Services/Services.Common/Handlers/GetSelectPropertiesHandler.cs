using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services
{
    class GetSelectPropertiesHandler<TEntity, TInterface, TId> : IGetPropertyValueHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;
        private readonly IEntityInfo<TEntity> _EntityInfo;

        public GetSelectPropertiesHandler(IRepository<TEntity, TInterface, TId> repository,
                                       IEntityInfo<TEntity> entityInfo)
        {
            _Repository = repository;
            _EntityInfo = entityInfo;
        }

        public string GetSelectProperty(TId id, string property)
        {
            if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo pi))
                throw new Exception($"Entity {typeof(TEntity).Name} does not have a property called {property}.");
            var entity = _Repository.GetSelectProperties(id, property);
            var propValue = pi.GetValue(entity);
            return propValue.ToString();
        }

        public TInterface GetSelectProperties(TId id, params string[] properties)
        {
            foreach (var property in properties)
            {
                if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo pi))
                    throw new Exception($"Entity {typeof(TEntity).Name} does not have a property called {property}.");
            }
            return _Repository.GetSelectProperties(id, properties);
        }
    }
}