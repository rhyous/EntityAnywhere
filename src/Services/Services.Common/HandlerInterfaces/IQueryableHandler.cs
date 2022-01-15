using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IQueryableHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        IQueryable<TInterface> GetQueryable();
        Task<IQueryable<TInterface>> GetQueryableAsync(NameValueCollection parameters);
        IQueryable<TInterface> GetQueryable(Expression<Func<TEntity, bool>> expression,
                                            int take = -1,
                                            int skip = -1,
                                            string sortProperty = "Id",
                                            SortOrder sortOrder = SortOrder.Ascending);
        List<TInterface> GetQueryableWithModifier(Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier, Expression<Func<TEntity, bool>> expression);
        IQueryable<TInterface> GetQueryable(IQueryable<TEntity> queryable, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TId>> orderExpression, int take = -1, int skip = -1);
    }
}
