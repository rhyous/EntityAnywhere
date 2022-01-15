using LinqKit;
using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public class QueryableHandler<TEntity, TInterface, TId> : IQueryableHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;
        private readonly IEntityConfigurationProvider _EntityConfigurationProvider;
        private readonly IFilterExpressionParser<TEntity> _FilterExpressionParser;
        private readonly ICustomFilterConvertersRunner<TEntity> _CustomFilterConvertersRunner;

        public QueryableHandler(IRepository<TEntity, TInterface, TId> repository,
                                IEntityConfigurationProvider entityConfigurationProvider,
                                IFilterExpressionParser<TEntity> filterExpressionParser,
                                ICustomFilterConvertersRunner<TEntity> customFilterConvertersRunner)
        {
            _Repository = repository;
            _EntityConfigurationProvider = entityConfigurationProvider;
            _FilterExpressionParser = filterExpressionParser;
            _CustomFilterConvertersRunner = customFilterConvertersRunner;
        }

        public IQueryable<TInterface> GetQueryable()
        {
            var config = _EntityConfigurationProvider.Provide(typeof(TEntity));
            return _Repository.Get(config.DefaultSortByProperty, config.DefaultSortOrder);
        }

        public async Task<IQueryable<TInterface>> GetQueryableAsync(NameValueCollection parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return GetQueryable();
            var expression = await _FilterExpressionParser.ParseAsync(parameters, true, _CustomFilterConvertersRunner);
            var entityConfiguration = _EntityConfigurationProvider.Provide(typeof(TEntity));
            var sortProperty = parameters.GetSortByProperty(typeof(TEntity), entityConfiguration.DefaultSortByProperty);
            var sortOrder = parameters.GetSortOrder(entityConfiguration.DefaultSortOrder);
            return GetQueryable(expression, parameters.Get("$top", -1), parameters.Get("$skip", -1), sortProperty, sortOrder);
        }

        public IQueryable<TInterface> GetQueryable(Expression<Func<TEntity, bool>> expression,
                                                   int take = -1,
                                                   int skip = -1,
                                                   string sortProperty = "Id",
                                                   SortOrder sortOrder = SortOrder.Ascending)
        {
            return _Repository.GetByExpression(expression ?? PredicateBuilder.New<TEntity>(true), sortProperty, sortOrder)
                              .IfSkip(skip)
                              .IfTake(take);
        }
        public List<TInterface> GetQueryableWithModifier(Func<IQueryable<TInterface>, IEnumerable<TInterface>> queryableModifier, Expression<Func<TEntity, bool>> expression)
        {
            var queryable = GetQueryable(expression ?? PredicateBuilder.New<TEntity>(true));
            if (queryable == null)
                return null;
            return queryableModifier == null ? queryable.ToList() : queryableModifier(queryable).ToList();
        }

        public IQueryable<TInterface> GetQueryable(IQueryable<TEntity> queryable, Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TId>> orderExpression, int take = -1, int skip = -1)
        {
            if (queryable == null)
                return null;
            if (expression != null)
                queryable = queryable.Where(expression);
            return queryable.OrderBy(orderExpression ?? (e => e.Id)).IfSkip(skip).IfTake(take);
        }
    }
}