using Rhyous.Collections;
using Rhyous.Odata.Filter;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    class GetByIdsHandler<TEntity, TInterface, TId> : IGetByIdsHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;
        private readonly IQueryableHandler<TEntity, TInterface, TId> _QueryableHandler;
        private readonly IFilterExpressionParser<TEntity> _FilterExpressionParser;
        private readonly ICustomFilterConvertersRunner<TEntity> _CustomFilterConvertersRunner;

        public GetByIdsHandler(IRepository<TEntity, TInterface, TId> repository,
                               IQueryableHandler<TEntity, TInterface, TId> queryableHandler,
                               IFilterExpressionParser<TEntity> filterExpressionParser,
                               ICustomFilterConvertersRunner<TEntity> customFilterConvertersRunner)
        {
            _Repository = repository;
            _QueryableHandler = queryableHandler;
            _FilterExpressionParser = filterExpressionParser;
            _CustomFilterConvertersRunner = customFilterConvertersRunner;
        }

        public async Task<IQueryable<TInterface>> GetAsync(IEnumerable<TId> ids, NameValueCollection parameters = null)
        {
            var queryable = _Repository.Get(ids) as IQueryable<TEntity>;
            var filterExpression = parameters == null || parameters.Count == 0 || parameters.Get<string>("$filter", null) == null
                                 ? null
                                 : await _FilterExpressionParser.ParseAsync(parameters, false, _CustomFilterConvertersRunner);
            return _QueryableHandler.GetQueryable(queryable, filterExpression, null, parameters.Get("$top", -1), parameters.Get("$skip", -1));
        }
    }
}