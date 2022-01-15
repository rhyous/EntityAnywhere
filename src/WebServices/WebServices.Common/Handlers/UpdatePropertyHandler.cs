using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class UpdatePropertyHandler<TEntity, TInterface, TId> : IUpdatePropertyHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRelatedEntityEnforcer<TEntity> _RelatedEntityEnforcer;
        private readonly IDistinctPropertiesEnforcer<TEntity, TInterface, TId> _DistinctPropertiesEnforcer;
        private readonly IEntityEventAll<TEntity, TId> _EntityEvent;
        private readonly IInputValidator<TEntity, TId> _InputValidator;
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;
        private readonly IEntityInfo<TEntity> _EntityInfo;

        public UpdatePropertyHandler(IRelatedEntityEnforcer<TEntity> relatedEntityEnforcer,
                                     IDistinctPropertiesEnforcer<TEntity, TInterface, TId> distinctPropertiesEnforcer,
                                     IEntityEventAll<TEntity, TId> entityEvent, 
                                     IInputValidator<TEntity, TId> inputValidator,
                                     IServiceCommon<TEntity, TInterface, TId> service,
                                     IEntityInfo<TEntity> entityInfo)
        {
            _RelatedEntityEnforcer = relatedEntityEnforcer ?? throw new ArgumentNullException(nameof(relatedEntityEnforcer));
            _DistinctPropertiesEnforcer = distinctPropertiesEnforcer ?? throw new ArgumentNullException(nameof(distinctPropertiesEnforcer));
            _EntityEvent = entityEvent ?? throw new ArgumentNullException(nameof(entityEvent));
            _InputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _EntityInfo = entityInfo;
        }

        public async Task<string> Handle(string id, string property, string value)
        {
            if (!_InputValidator.CleanAndValidate(typeof(TEntity), ref id, ref property, ref value))
                throw new RestException(HttpStatusCode.BadRequest);
            if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo pi))
                throw new RestException($"Entity '{typeof(TEntity).Name}' does not have a '{property}' property.", HttpStatusCode.BadRequest);

            var existingEntity = _Service.Get(id.To<TId>())?.ToConcrete<TEntity, TInterface>();
            if (existingEntity == null)
                throw new RestException(HttpStatusCode.NotFound);

            var existingPropertyValue = pi.GetValue(existingEntity);
            pi.SetValue(existingEntity, value.ToType(pi.PropertyType));

            await _RelatedEntityEnforcer.Enforce(new[] { existingEntity });    // Check to be sure RelatedEntities actually exist
            await _DistinctPropertiesEnforcer.Enforce(new[] { existingEntity }, ChangeType.Update);
            _EntityEvent?.BeforeUpdateProperty(property, value, existingPropertyValue);
            var result = _Service.UpdateProperty(id.To<TId>(), property, value);
            _EntityEvent?.AfterUpdateProperty(property, result, existingPropertyValue);
            return result;
        }
    }
}