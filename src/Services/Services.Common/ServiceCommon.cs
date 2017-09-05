using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// A common service layer for all Entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TInterface">The entity interface type.</typeparam>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public class ServiceCommon<T, Tinterface, Tid> : IServiceCommon<T, Tinterface, Tid>
        where T: class, Tinterface, new()
        where Tinterface : IId<Tid>
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        public virtual IRepository<T, Tinterface, Tid> Repo
        {
            get { return _Repo ?? (_Repo = RepositoryLoader.Load<T, Tinterface, Tid>()); }
            set { _Repo = value; }
        } private IRepository<T, Tinterface, Tid> _Repo;

        /// <inheritdoc />
        public virtual List<Tinterface> Get(List<Tid> ids)
        {
            return Repo.Get(ids).ToList();
        }

        /// <inheritdoc />
        public virtual List<Tinterface> Get()
        {
            return Repo.Get().ToList();
        }

        /// <inheritdoc />
        public virtual Tinterface Get(Tid Id)
        {
            return Repo.Get(Id);
        }

        /// <inheritdoc />
        public virtual List<Tinterface> Get(NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual List<Tinterface> Get(Expression<Func<Tinterface, bool>> expression)
        {
            return Repo.GetByExpression(expression).ToList();
        }

        /// <inheritdoc />
        public virtual string GetProperty(Tid Id, string property)
        {
            return Repo.Get(Id)?.GetPropertyValue(property)?.ToString();
        }

        /// <inheritdoc />
        public virtual string UpdateProperty(Tid id, string property, string value)
        {
            var entity = new T() { Id = id};
            var changedProperties = new List<string>() { property };
            var type = entity.GetPropertyInfo(property).PropertyType;
            var typedValue = Convert.ChangeType(value, type);
            entity.GetPropertyInfo(property).SetValue(entity, typedValue);
            return Update(id, entity, changedProperties).GetPropertyValue(property).ToString();
        }

        /// <inheritdoc />
        public virtual List<Tinterface> Add(IList<Tinterface> entities)
        {
            return Repo.Create(entities);
        }

        /// <inheritdoc />
        public virtual Tinterface Add(Tinterface entity)
        {
            return Repo.Create(new[] { entity }).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual Tinterface Update(Tid id, Tinterface entity, List<string> changedProperties)
        {
            entity.Id = id;
            return Repo.Update(entity, changedProperties);
        }

        /// <inheritdoc />
        public virtual Tinterface Replace(Tid Id, Tinterface entity)
        {
            var allProperties = from prop in typeof(Tinterface).GetProperties()
                                where prop.CanRead && prop.CanWrite && prop.Name != "Id"
                                select prop.Name;
            return Repo.Update(entity, allProperties);
        }

        /// <inheritdoc />
        public virtual bool Delete(Tid id)
        {
            return Repo.Delete(id);
        }        
    }
}