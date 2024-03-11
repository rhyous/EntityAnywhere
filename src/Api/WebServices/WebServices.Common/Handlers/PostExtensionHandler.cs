using Newtonsoft.Json;
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
    class PostExtensionHandler<TEntity, TInterface, TId> : IPostExtensionHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IGetByIdHandler<TEntity, TInterface, TId> _GetByIdHandler;
        private readonly INamedFactory<IAdminExtensionEntityClientAsync> _NamedFactory;
        private readonly IHttpStatusCodeSetter _HttpStatusCodeSetter;

        public PostExtensionHandler(IGetByIdHandler<TEntity, TInterface, TId> getByIdHandler,
                                    INamedFactory<IAdminExtensionEntityClientAsync> namedFactory,
                                    IHttpStatusCodeSetter httpStatusCodeSetter)
        {
            _GetByIdHandler = getByIdHandler;
            _NamedFactory = namedFactory;
            _HttpStatusCodeSetter = httpStatusCodeSetter;
        }

        public async Task<OdataObject<TEntity, TId>> HandleAsync(string id, string extensionEntity, PropertyValue propertyValue)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(extensionEntity) || propertyValue == null || string.IsNullOrWhiteSpace(propertyValue.Property))
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            var odataEntity = await _GetByIdHandler.HandleAsync(id);
            if (odataEntity == null)
            {
                _HttpStatusCodeSetter.StatusCode = HttpStatusCode.NotFound;
                return null;
            }
            var extensionEntityClient = _NamedFactory.Create(extensionEntity);
            var extensionEntityPost = new
            {
                Entity = typeof(TEntity).Name,
                EntityId = odataEntity.Id.ToString(),
                Property = propertyValue.Property,
                value = propertyValue.Value
            };
            var array = new[] { extensionEntityPost };
            var jsonResult = await extensionEntityClient.PostAsync(array);
            if (string.IsNullOrWhiteSpace(jsonResult) || (!jsonResult.StartsWith("{") && !jsonResult.EndsWith("}")))
                throw new RestException("Unknown error created the extension entity", HttpStatusCode.InternalServerError);
            var relatedEntityCollection = odataEntity.RelatedEntityCollection.FirstOrDefault(rec => rec.RelatedEntity == extensionEntity);
            if (relatedEntityCollection == null)
            {
                relatedEntityCollection = new RelatedEntityCollection { Entity = typeof(TEntity).Name, RelatedEntity = extensionEntity };
                odataEntity.RelatedEntityCollection.Add(relatedEntityCollection);
            }
            var odataObjectCollection = JsonConvert.DeserializeObject<OdataObjectCollection>(jsonResult);
            relatedEntityCollection.Add(odataObjectCollection[0]);
            return odataEntity;
        }
    }
}