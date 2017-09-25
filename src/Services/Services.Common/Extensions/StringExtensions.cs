using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    public static class StringExtensions
    {
        public static Expression<Func<T, bool>> ToExpression<T>(this string filterString, Type compareType)
        {
            var method = typeof(StringExtensions).GetMethods().Where(m => m.Name == "ToExpression" && m.IsGenericMethod && m.GetParameters().Length == 1).FirstOrDefault();
            method = method.MakeGenericMethod(typeof(T), compareType);
            return method.Invoke(null, new[] { filterString }) as Expression<Func<T, bool>>;
        }
    }
}
