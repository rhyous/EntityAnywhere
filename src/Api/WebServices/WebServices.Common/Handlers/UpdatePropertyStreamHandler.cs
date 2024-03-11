using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using Rhyous.Wrappers;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class UpdatePropertyStreamHandler<TEntity, TInterface, TId> : IUpdatePropertyStreamHandler<TEntity, TInterface, TId>
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
        private readonly IHttpStatusCodeSetter _HttpStatusCodeSetter;

        public UpdatePropertyStreamHandler(IRelatedEntityEnforcer<TEntity> relatedEntityEnforcer,
                                     IDistinctPropertiesEnforcer<TEntity, TInterface, TId> distinctPropertiesEnforcer,
                                     IEntityEventAll<TEntity, TId> entityEvent, 
                                     IInputValidator<TEntity, TId> inputValidator,
                                     IServiceCommon<TEntity, TInterface, TId> service,
                                     IEntityInfo<TEntity> entityInfo,
                                     IHttpStatusCodeSetter httpStatusCodeSetter)
        {
            _RelatedEntityEnforcer = relatedEntityEnforcer ?? throw new ArgumentNullException(nameof(relatedEntityEnforcer));
            _DistinctPropertiesEnforcer = distinctPropertiesEnforcer ?? throw new ArgumentNullException(nameof(distinctPropertiesEnforcer));
            _EntityEvent = entityEvent ?? throw new ArgumentNullException(nameof(entityEvent));
            _InputValidator = inputValidator ?? throw new ArgumentNullException(nameof(inputValidator));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
            _EntityInfo = entityInfo;
            _HttpStatusCodeSetter = httpStatusCodeSetter;
        }

        public async Task<bool> HandleAsync(string id, string property, Stream value)
        {
            if (!_InputValidator.CleanAndValidate(typeof(TEntity), ref id, ref property))
                throw new RestException(HttpStatusCode.BadRequest);
            if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo pi))
                throw new RestException($"Entity '{typeof(TEntity).Name}' does not have a '{property}' property.", HttpStatusCode.BadRequest);

            var existingEntityId = _Service.GetPropertyValue(id.To<TId>(), "Id").ToString();
            if (string.IsNullOrWhiteSpace(existingEntityId))
            {
                _HttpStatusCodeSetter.StatusCode = HttpStatusCode.NotFound;
                return false;
            }
            _EntityEvent?.BeforeUpdateProperty(property, "new stream", "old stream");
            var result = await _Service.UpdateStreamPropertyAsync(id.To<TId>(), property, value);
            _EntityEvent?.AfterUpdateProperty(property, "new stream", "old stream");
            return result;
        }
    }
}