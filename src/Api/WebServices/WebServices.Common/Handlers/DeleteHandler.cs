using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Net;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class DeleteHandler<TEntity, TInterface, TId> : IDeleteHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IEntityEventAll<TEntity, TId> _EntityEvent;
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;

        public DeleteHandler(IEntityEventAll<TEntity, TId> entityEvent,
                             IServiceCommon<TEntity, TInterface, TId> service)
        {
            _EntityEvent = entityEvent ?? throw new ArgumentNullException(nameof(entityEvent));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public bool Handle(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new RestException(HttpStatusCode.BadRequest);
            return Handle(id.To<TId>());
        }

        public bool Handle(TId id)
        {
            var existingEntity = _Service.Get(id)?.ToConcrete<TEntity, TInterface>();
            if (existingEntity == null) // Already deleted or was never there
                return true;
            _EntityEvent?.BeforeDelete(existingEntity);
            var result = _Service.Delete(id);
            _EntityEvent?.AfterDelete(existingEntity, result);
            return result;
        }
    }
}