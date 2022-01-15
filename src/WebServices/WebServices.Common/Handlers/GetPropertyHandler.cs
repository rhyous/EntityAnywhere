using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System;
using System.Net;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class GetPropertyHandler<TEntity, TInterface, TId> : IGetPropertyHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IWebOperationContext _WebOperationContext;
        private readonly IEntityInfo<TEntity> _EntityInfo;

        public GetPropertyHandler(IServiceCommon<TEntity, TInterface, TId> service,
                                  IWebOperationContext webOperationContext,
                                  IEntityInfo<TEntity> entityInfo)
        {
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _WebOperationContext = webOperationContext;
            _EntityInfo = entityInfo;
        }

        public string Handle(string id, string property)
        {
            var tId = id.To<TId>();
            var entity = _Service.Get(tId);
            if (entity == null)
            {
                _WebOperationContext.OutgoingResponse.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
            if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo propInfo))
            {
                throw new RestException($"Entity '{typeof(TEntity).Name}' does not have a '{property}' property.", HttpStatusCode.BadRequest);
            }
            return propInfo.GetValue(entity).ToString();
        }
    }
}