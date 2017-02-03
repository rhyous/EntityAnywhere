using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
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

        public virtual List<Tinterface> Get(List<Tid> ids)
        {
            return Repo.Get(ids).ToList();
        }

        public virtual List<Tinterface> Get()
        {
            return Repo.Get().ToList();
        }

        public virtual Tinterface Get(Tid Id)
        {
            return Repo.Get(Id);
        }

        public virtual List<Tinterface> Get(Expression<Func<Tinterface, bool>> expression)
        {
            return Repo.GetByExpression(expression).ToList();
        }

        public virtual string GetProperty(Tid Id, string property)
        {
            return Repo.Get(Id)?.GetPropertyValue(property)?.ToString();
        }

        public virtual List<Tinterface> Add(IList<Tinterface> entities)
        {
            return Repo.Create(entities);
        }

        public virtual Tinterface Update(Tid Id, Tinterface entity, List<string> changedProperties)
        {
            return Repo.Update(entity, changedProperties);
        }

        public virtual Tinterface Replace(Tid Id, Tinterface entity)
        {
            var allProperties = from prop in typeof(Tinterface).GetProperties()
                                where prop.CanRead && prop.CanWrite && prop.Name != "Id"
                                select prop.Name;
            return Repo.Update(entity, allProperties);
        }

        public virtual bool Delete(Tid id)
        {
            return Repo.Delete(id);
        }        
    }
}