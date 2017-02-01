using LinqKit;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Repositories
{
    public class BaseRepository<T, Tinterface, Tid> : IRepository<T, Tinterface, Tid>
        where Tinterface : IEntity<Tid>
        where T : class, Tinterface, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        protected BaseDbContext<T> DbContext
        {
            get { return _DbContext ?? (DbContext = new BaseDbContext<T>()); }
            set { _DbContext = value; }
        } private BaseDbContext<T> _DbContext;

        public virtual List<Tinterface> Create(IList<Tinterface> items)
        {
            List<Tinterface> result = new List<Tinterface>();
            if (items != null && items.Count > 0)
            {
                var concrete = ConcreteConverter.ToConcrete<T, Tinterface>(items);
                result.AddRange(DbContext.Entities.AddRange(concrete));
                DbContext.SaveChanges();
            }
            return result;
        }

        public virtual bool Delete(Tid id)
        {
            var item = DbContext.Entities.FirstOrDefault(o => o.Id.Equals(id));
            if (item == null)
                return true;
            DbContext.Entities.Remove(item);
            DbContext.SaveChanges();
            return true;
        }

        public virtual List<Tinterface> Get()
        {
            return DbContext.Entities.ToList<Tinterface>();
        }

        public virtual List<Tinterface> Get(List<Tid> ids)
        {
            return DbContext.Entities.Where(o => ids.Contains(o.Id)).ToList<Tinterface>();
        }

        public virtual Tinterface Get(Tid id)
        {
            return DbContext.Entities.FirstOrDefault(o => o.Id.Equals(id));
        }

        public virtual Tinterface Get(string name, Expression<Func<T, string>> propertyExpression)
        {
            return DbContext.Entities.AsExpandable().FirstOrDefault(e => propertyExpression.Invoke(e) == name);
        }

        public virtual List<Tinterface> GetByExpression(Expression<Func<T, bool>> expression)
        {
            return DbContext.Entities.AsExpandable().Where(expression).ToList<Tinterface>();
        }

        public virtual List<Tinterface> Search(string searchString, params Expression<Func<T, string>>[] propertyExpressions)
        {
            var predicate = PredicateBuilder.New<T>();
            foreach (var expression in propertyExpressions)
            {
                predicate.Or(e => expression.Invoke(e).Contains(searchString));
            }
            return GetByExpression(predicate);
        }

        public virtual Tinterface Update(Tinterface item, IEnumerable<string> changedProperties)
        {
            var existingItem = DbContext.Entities.FirstOrDefault(o => o.Id.Equals(item.Id));
            foreach (var prop in changedProperties)
            {
                var value = item.GetPropertyValue(prop);
                existingItem.GetPropertyInfo(prop).SetValue(existingItem, value);
            }
            DbContext.SaveChanges();
            return existingItem;
        }
    }
}
