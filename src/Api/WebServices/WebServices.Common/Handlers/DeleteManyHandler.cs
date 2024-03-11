using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class DeleteManyHandler<TEntity, TInterface, TId> : IDeleteManyHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IEntityEventAll<TEntity, TId> _EntityEvent;
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;

        public DeleteManyHandler(IEntityEventAll<TEntity, TId> entityEvent,
                                 IServiceCommon<TEntity, TInterface, TId> service)
        {
            _EntityEvent = entityEvent ?? throw new ArgumentNullException(nameof(entityEvent));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<Dictionary<TId, bool>> HandleAsync(IEnumerable<string> ids)
        {
            if (ids == null || !ids.Any())
                throw new RestException(HttpStatusCode.BadRequest);
            var typedIds = ids.Where(id => !string.IsNullOrWhiteSpace(id)).Select(id => id.To<TId>());
            var existingEntities = (await _Service.GetAsync(typedIds, null))?.ToConcrete<TEntity, TInterface>();            
            if (existingEntities == null || !existingEntities.Any()) // They are already deleted or were never there. Skip events.
                return typedIds.ToDictionary(ti => ti, ti => false);
            _EntityEvent?.BeforeDeleteMany(existingEntities);
            var result = _Service.DeleteMany(typedIds);
            _EntityEvent?.AfterDeleteMany(existingEntities, result);
            return result;
        }
    }
}