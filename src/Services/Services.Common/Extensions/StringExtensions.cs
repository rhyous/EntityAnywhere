using System;
using System.Linq.Expressions;

namespace Rhyous.WebFramework.Services
{
    public static class StringExtensions
    {
        public static Expression<Func<E, Eout>> ToLambda<E, Eout>(this string propertyName)
        {
            var param = Expression.Parameter(typeof(E));
            var body = Expression.PropertyOrField(param, propertyName);
            return Expression.Lambda<Func<E, Eout>>(body, param);
        }

        public static Expression<Func<E, bool>> ToLambda<E, V>(this string propertyName, V value, string methodName = "Equals")
        {
            ParameterExpression parameter = Expression.Parameter(typeof(E), "e");
            Expression property = Expression.Property(parameter, propertyName);
            Expression target = Expression.Constant(value);
            Expression method = Expression.Call(property, methodName, null, target);
            return Expression.Lambda<Func<E, bool>>(method, parameter);
        }
    }
}
