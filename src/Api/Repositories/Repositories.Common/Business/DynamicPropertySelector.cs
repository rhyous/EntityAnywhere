using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>This came from: https://stackoverflow.com/a/58719125/375727</remarks>
    internal class DynamicPropertySelector<TEntity> : IDynamicPropertySelector<TEntity>
    {
        public Expression<Func<TEntity, TSelect>> DynamicSelectGenerator<TSelect>(params string[] properties)
        {
            if (properties == null || properties.Length == 0)
                throw new ArgumentNullException(nameof(properties));

            // input parameter "x"
            var xParameter = Expression.Parameter(typeof(TEntity), "x");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(TSelect));

            // create initializers
            var bindings = properties
                .Select(x =>
                {
                    string[] xFieldAlias = x.Split(':');
                    string field = xFieldAlias[0];

                    string[] fieldSplit = field.Split('.');
                    if (fieldSplit.Length > 1)
                    {
                        // original value "x.Nested.Field1"
                        Expression exp = xParameter;
                        foreach (string item in fieldSplit)
                            exp = Expression.PropertyOrField(exp, item);

                        // property "Field1"
                        PropertyInfo member2 = null;
                        if (xFieldAlias.Length > 1)
                            member2 = typeof(TSelect).GetProperty(xFieldAlias[1]);
                        else
                            member2 = typeof(TEntity).GetProperty(fieldSplit[fieldSplit.Length - 1]);

                        // set value "Field1 = x.Nested.Field1"
                        var res = Expression.Bind(member2, exp);
                        return res;
                    }
                    // property "Field1"
                    var mi = typeof(TEntity).GetProperty(field);
                    PropertyInfo member;
                    if (xFieldAlias.Length > 1)
                        member = typeof(TSelect).GetProperty(xFieldAlias[1]);
                    else member = typeof(TSelect).GetProperty(field);

                    // original value "x.Field1"
                    var xOriginal = Expression.Property(xParameter, mi);

                    // set value "Field1 = x.Field1"
                    return Expression.Bind(member, xOriginal);
                }
            );

            // initialization "new Data { Field1 = x.Field1, Field2 = x.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "x => new Data { Field1 = x.Field1, Field2 = x.Field2 }"
            var lambda = Expression.Lambda<Func<TEntity, TSelect>>(xInit, xParameter);

            return lambda;
        }
    }
}
