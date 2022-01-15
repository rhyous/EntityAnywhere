using LinqKit;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetByEntityIdentifiers<TEntity, TInterface, TId> : IGetByEntityIdentifiers<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IExtensionEntity, IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRequestUri _RequestUri;

        public GetByEntityIdentifiers(IServiceCommon<TEntity, TInterface, TId> service,
                                      IRequestUri requestUri)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public OdataObjectCollection<TEntity, TId> Handle(IEnumerable<EntityIdentifier> entityIdentifiers)
        {
            if (entityIdentifiers == null || !entityIdentifiers.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            var expression = entityIdentifiers.ToExpression<TEntity, TInterface, TId>();
            return _Service.Get(expression).ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(_RequestUri.Uri);
        }

        public OdataObjectCollection<TEntity, TId> Handle(string entity, List<string> entityIds)
        {
            if (string.IsNullOrWhiteSpace(entity) || entityIds == null || !entityIds.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            var expression = PredicateBuilder.New<TEntity>(e => e.Entity == entity && entityIds.Contains(e.EntityId));
            return _Service.Get(expression).ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(_RequestUri.Uri);
        }
    }
}