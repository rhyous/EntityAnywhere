using LinqKit;
using Rhyous.Collections;
using Rhyous.Odata.Filter;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    class GetByPropertyValuesHandler<TEntity, TInterface, TId> : IGetByPropertyValuesHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IQueryableHandler<TEntity, TInterface, TId> _QueryableHandler;
        private readonly IFilterExpressionParser<TEntity> _FilterExpressionParser;
        private readonly ICustomFilterConvertersRunner<TEntity> _CustomFilterConvertersRunner;
        private readonly IEntityInfo<TEntity> _EntityInfo;

        public GetByPropertyValuesHandler(IQueryableHandler<TEntity, TInterface, TId> queryableHandler,
                                          IFilterExpressionParser<TEntity> filterExpressionParser,
                                          ICustomFilterConvertersRunner<TEntity> customFilterConvertersRunner,
                                          IEntityInfo<TEntity> entityInfo)
        {
            _QueryableHandler = queryableHandler;
            _FilterExpressionParser = filterExpressionParser;
            _CustomFilterConvertersRunner = customFilterConvertersRunner;
            _EntityInfo = entityInfo;
        }

        public virtual async Task<IQueryable<TInterface>> GetAsync(string property, IEnumerable<string> values, NameValueCollection parameters)
        {
            if (string.IsNullOrWhiteSpace(property) || values == null || !values.Any())
                return null;
            if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo propInfo))
                throw new ArgumentException($"The property parameter must be a valid property of {typeof(TEntity).FullName}", "property");
            MethodInfo method = GetType().GetMethod(nameof(GetByPropertyValuesAsync), BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo genericMethod = method.MakeGenericMethod(propInfo.PropertyType);
            var typedList = values.Select(v => v.ToType(propInfo.PropertyType));
            var methodParams = new object[] { propInfo.Name, typedList, parameters };
            return await genericMethod.InvokeAsync<IQueryable<TInterface>>(this, methodParams);
        }

        public async Task<IQueryable<TInterface>> GetAsync<T>(string property, IEnumerable<T> values, NameValueCollection parameters = null)
        {
            var typedList = values.ToList();
            var expressionStarter = PredicateBuilder.New(property.ToLambda<TEntity, T>(typedList));
            var filterExpression = parameters == null || parameters.Count == 0 || parameters.Get<string>("$filter", null) == null
                                 ? null
                                 : await _FilterExpressionParser.ParseAsync(parameters, false, _CustomFilterConvertersRunner);
            if (filterExpression != null)
                expressionStarter.And(filterExpression);
            return _QueryableHandler.GetQueryable(expressionStarter, parameters.Get("$top", -1), parameters.Get("$skip", -1));
        }

        internal async Task<IQueryable<TInterface>> GetByPropertyValuesAsync<T>(string property, IEnumerable<object> values, NameValueCollection parameters = null)
        {
            var typedList = values.Cast<T>().ToList();
            return await GetAsync<T>(property, typedList, parameters);
        }
    }
}
