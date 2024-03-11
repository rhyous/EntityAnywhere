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
    /// <summary>Get Extension entities by property value pairs.</summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public class GetByPropertyValuePairsHandler<TEntity, TInterface, TId>
        : IGetByPropertyValuePairsHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IExtensionEntity, IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IRequestUri _RequestUri;

        public GetByPropertyValuePairsHandler(IServiceCommon<TEntity, TInterface, TId> service,
                                              IRequestUri requestUri)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _RequestUri = requestUri ?? throw new ArgumentNullException(nameof(requestUri));
        }

        public OdataObjectCollection<TEntity, TId> Handle(IEnumerable<PropertyValue> propertyValues)
        {
            if (propertyValues == null || !propertyValues.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            var expression = propertyValues.ToExpression<TEntity, TInterface, TId>();
            return _Service.Get(expression).ToConcrete<TEntity, TInterface>().ToList().AsOdata<TEntity, TId>(_RequestUri.Uri);
        }
    }
}