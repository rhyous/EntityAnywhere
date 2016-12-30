using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Rhyous.WebFramework.Services
{
    public static class StringExtensions
    {
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
