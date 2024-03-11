using System;
using System.Linq.Expressions;

namespace Rhyous.EntityAnywhere.Repositories
{
    public interface IDynamicPropertySelector<TEntity>
    {
        Expression<Func<TEntity, TSelect>> DynamicSelectGenerator<TSelect>(params string[] properties);
    }
}