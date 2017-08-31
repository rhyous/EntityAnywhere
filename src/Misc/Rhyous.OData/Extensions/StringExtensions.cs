using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.OData
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
        
        public static Expression<Func<E, bool>> ToLambda<E, Tid, V>(this string propertyName, List<Tid> values)
        {
            var methodInfo = typeof(List<Tid>).GetMethod("Contains",  new Type[] { typeof(Tid) });
            var list = Expression.Constant(values);
            var param = Expression.Parameter(typeof(E), "e");
            var value = Expression.Property(param, propertyName);
            var body = Expression.Call(list, methodInfo, value);            
            return Expression.Lambda<Func<E, bool>>(body, param);
        }
    }
}
