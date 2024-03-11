using Newtonsoft.Json;
using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;



namespace Rhyous.EntityAnywhere.WebServices
{
    class DeleteExtensionHandler<TEntity, TInterface, TId> : IDeleteExtensionHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IGetByIdHandler<TEntity, TInterface, TId> _GetByIdHandler;
        private readonly INamedFactory<IAdminExtensionEntityClientAsync> _NamedFactory;
        private readonly IExtensionEntityList _ExtensionEntityList;
        private readonly IHttpStatusCodeSetter _HttpStatusCodeSetter;

        public DeleteExtensionHandler(IGetByIdHandler<TEntity, TInterface, TId> getByIdHandler,
                                      INamedFactory<IAdminExtensionEntityClientAsync> namedFactory,
                                      IExtensionEntityList extensionEntityList,
                                      IHttpStatusCodeSetter httpStatusCodeSetter)
        {
            _GetByIdHandler = getByIdHandler;
            _NamedFactory = namedFactory;
            _ExtensionEntityList = extensionEntityList;
            _HttpStatusCodeSetter = httpStatusCodeSetter;
        }
        public async Task<Dictionary<long, bool>> HandleAsync(string id, string extensionEntity)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(extensionEntity))
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            var odataEntity = await _GetByIdHandler.HandleAsync(id);
            if (odataEntity == null)
            {
                _HttpStatusCodeSetter.StatusCode = HttpStatusCode.NotFound;
                return null;
            }

            Dictionary<long, bool> results = new Dictionary<long, bool>();
            var relatedEntities = odataEntity.RelatedEntityCollection.FirstOrDefault(rec => rec.RelatedEntity == extensionEntity);

            if (extensionEntity.ToLower() == "all")
            {
                foreach(string eName in _ExtensionEntityList.EntityNames)
                {
                    relatedEntities = odataEntity.RelatedEntityCollection.FirstOrDefault(rec => rec.RelatedEntity == eName);
                    var relatedEntityIds = relatedEntities.Select(re => re.Id.To<long>());
                    
                    results.AddRange(await DeleteAsync(eName, relatedEntityIds));
                }
            } 
            else 
            {
                var relatedEntityIds = relatedEntities.Select(re => re.Id.To<long>());

                results.AddRange(await DeleteAsync(extensionEntity, relatedEntityIds));
            }

            return results;
        }

        public async Task<Dictionary<long, bool>> HandleAsync(string id, string extensionEntity, IEnumerable<string> extentionEntityIds)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(extensionEntity))
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }

            var odataEntity = await _GetByIdHandler.HandleAsync(id);
            if (odataEntity == null)
            {
                _HttpStatusCodeSetter.StatusCode = HttpStatusCode.NotFound;
                return null;
            }

            var relatedEntities = odataEntity.RelatedEntityCollection.FirstOrDefault(rec => rec.RelatedEntity == extensionEntity);
            IEnumerable<long> relatedEntityIds = null;
            if (relatedEntities != null)
            {
                relatedEntityIds = relatedEntities.Where(re => JsonConvert.DeserializeObject<ExtensionEntity>(re.Object.ToString()).Entity == typeof(TEntity).Name &&
                                                               extentionEntityIds.Any(eeid => eeid == re.Id))
                                                  .Select(re => re.Id.To<long>());
            }

            var results = await DeleteAsync(extensionEntity, relatedEntityIds);

            foreach (string eeid in extentionEntityIds)
            {
                if (!results.ContainsKey(eeid.To<long>()))
                    results.Add(eeid.To<long>(), false);
            }

            return null;
        }

        internal async Task<Dictionary<long, bool>> DeleteAsync(string extensionEntity, IEnumerable<long> extentionEntityIds)
        {
            var extensionEntityClient = _NamedFactory.Create(extensionEntity);
            return await extensionEntityClient.DeleteManyAsync(extentionEntityIds, true);
        }
    }
}
