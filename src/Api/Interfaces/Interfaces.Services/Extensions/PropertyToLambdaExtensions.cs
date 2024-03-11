using Rhyous.StringLibrary;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class PropertyToLambdaExtensions
    {
        public static object GetOrderByExpression<TEntity>(this string orderBy, PropertyInfo property)
        {
            var toLambdaGeneric = typeof(PropertyNameLambdaExtensions)
                                  .GetMethods()
                                  .FirstOrDefault(m => m.Name == nameof(PropertyNameLambdaExtensions.ToLambda)
                                                    && m.GetParameters().Length == 1
                                                    && m.GetParameters()[0].ParameterType == typeof(string)
                                                    && m.GetGenericArguments().Length == 2);
            var concreteMethodInfo = toLambdaGeneric.MakeGenericMethod(typeof(TEntity), property.PropertyType);
            var orderByExpression = concreteMethodInfo.Invoke(null, new[] { orderBy });
            return orderByExpression;
        }
    }
}
