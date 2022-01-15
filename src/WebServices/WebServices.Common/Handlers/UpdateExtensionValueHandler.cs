using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    class UpdateExtensionValueHandler<TEntity, TInterface, TId> : IUpdateExtensionValueHandler<TEntity, TInterface, TId>
            where TEntity : class, TInterface, new()
            where TInterface : IBaseEntity<TId>
            where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IGetByIdHandler<TEntity, TInterface, TId> _GetByIdHandler;
        private readonly INamedFactory<IAdminExtensionEntityClientAsync> _NamedFactory;
        private readonly IOutgoingWebResponseContext _OutgoingWebResponseContext;

        public UpdateExtensionValueHandler(IGetByIdHandler<TEntity, TInterface, TId> getByIdHandler,
                                    INamedFactory<IAdminExtensionEntityClientAsync> namedFactory,
                                    IOutgoingWebResponseContext outgoingWebResponseContext)
        {
            _GetByIdHandler = getByIdHandler;
            _NamedFactory = namedFactory;
            _OutgoingWebResponseContext = outgoingWebResponseContext;
        }

        public async Task<string> HandleAsync(string id, string extensionEntity, PropertyValue propertyValue)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(extensionEntity) || propertyValue == null || string.IsNullOrWhiteSpace(propertyValue.Property))
            {
                _OutgoingWebResponseContext.StatusCode = HttpStatusCode.BadRequest;
                return null;
            }
            var odataEntity = await _GetByIdHandler.HandleAsync(id);
            if (odataEntity == null)
                throw new RestException(HttpStatusCode.NotFound);
            var extensionEntityClient = _NamedFactory.Create(extensionEntity);
            var entityIdentifier = new EntityIdentifier { Entity = typeof(TEntity).Name, EntityId = id };
            var extensionEntitiesJson = await extensionEntityClient.GetByEntityIdentifiersAsync(new[] { entityIdentifier });
            if (string.IsNullOrWhiteSpace(extensionEntitiesJson) || (!extensionEntitiesJson.StartsWith("{") && extensionEntitiesJson.EndsWith("}")))
                throw new RestException(HttpStatusCode.NotFound);
            var odataEntities = JsonConvert.DeserializeObject<OdataObjectCollection>(extensionEntitiesJson);
            if (!odataEntities.Any())
                throw new RestException(HttpStatusCode.NotFound);
            var entityToUpdate = odataEntities.FirstOrDefault(o => JObject.Parse(o.Object.ToString())["Property"].ToString() == propertyValue.Property);
            var updated = await extensionEntityClient.UpdatePropertyAsync(entityToUpdate.Id, nameof(ExtensionEntity.Value), propertyValue.Value);
            return updated is null || updated.Length < 2
                   ? updated 
                   : updated.Substring(1, updated.Length - 2);// Remove JSON quotes
        }
    }
}