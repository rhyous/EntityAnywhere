using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Collections;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.Odata;
using Rhyous.StringLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class UserRoleEntityDataCache : CacheBase<ConcurrentDictionaryWrapper<int, IUserRoleEntityData>>,
                                           IUserRoleEntityDataCache
    {
        internal const string Entity = nameof(Entity);
        internal const string Name = nameof(Name);
        internal const string UserRole = nameof(UserRole);
        public const string Expand = "$Expand=Entity";

        private readonly INamedFactory<IAdminEntityClientAsync> _NamedFactory;

        public UserRoleEntityDataCache(INamedFactory<IAdminEntityClientAsync> namedFactory)
            : base(new ConcurrentDictionaryWrapper<int, IUserRoleEntityData>())
        {
            _NamedFactory = namedFactory;
        }

        /// <summary>Exposed internal for unit tests.</summary>
        internal ConcurrentDictionaryWrapper<int, IUserRoleEntityData> Cache => _Cache;

        public ConcurrentDictionary<string, int> UserRoleIds { get; } = new ConcurrentDictionary<string, int>();

        public IUserRoleEntityData this[string key]
        {
            get { return _Cache[UserRoleIds[key]]; }
        }

        protected override async Task CreateCacheAsync()
        {
            var client = _NamedFactory.Create(UserRole);
            var userRolesJson = await client.GetByQueryParametersAsync(Expand);
            if (string.IsNullOrWhiteSpace(userRolesJson) || (!userRolesJson.StartsWith("{") || !userRolesJson.EndsWith("}")))
                return;
            var odataObjectCollection = JsonConvert.DeserializeObject<OdataObjectCollection>(userRolesJson);
            foreach (var odataObject in odataObjectCollection)
            {
                var userRoleEntityData = CreateRoleEntityDataCache(odataObject);
                _Cache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);
                UserRoleIds.GetOrAdd(userRoleEntityData.UserRoleName, userRoleEntityData.UserRoleId);
            }
        }

        public override Task<ConcurrentDictionaryWrapper<int, IUserRoleEntityData>> ProvideAsync(bool forceUpdate = false)
        {
            return base.ProvideAsync(forceUpdate);
        }

        public bool TryGetValue(int key, out IUserRoleEntityData value)
        {
            value = null;
            return _Cache.TryGetValue(key, out value);
        }

        public bool TryGetValue(string key, out IUserRoleEntityData value)
        {
            value = null;
            return UserRoleIds.TryGetValue(key, out int intKey) && _Cache.TryGetValue(intKey, out value);
        }

        public bool Remove(int key) => _Cache.TryRemove(key, out _);

        public async Task UpdateRoleEntityDataAsync(int roleId)
        {
            var client = _NamedFactory.Create(UserRole);
            var userRoleJson = await client.GetAsync(roleId.ToString(), Expand);
            var odataObject = JsonConvert.DeserializeObject<OdataObject>(userRoleJson);
            var userRoleEntityData = CreateRoleEntityDataCache(odataObject);
            _Cache.TryRemove(roleId, out _);
            UserRoleIds.TryRemove(userRoleEntityData.UserRoleName, out _);
            _Cache.GetOrAdd(roleId, userRoleEntityData);
            UserRoleIds.TryAdd(userRoleEntityData.UserRoleName, userRoleEntityData.UserRoleId);
        }

        public IUserRoleEntityData CreateRoleEntityDataCache(OdataObject odataUserRole)
        {
            var userRoleEntityData = new UserRoleEntityData
            {
                UserRoleId = odataUserRole.Id.To<int>(),
                UserRoleName = JObject.Parse(odataUserRole.Object.ToString())["Name"].ToString()
            };
            // Don't be confused because the related entity is the entity named "Entity"
            var relatedEntities = odataUserRole.RelatedEntityCollection.FirstOrDefault(re => re.RelatedEntity == Entity);
            if (relatedEntities != null && relatedEntities.Any())
            {
                foreach (var entity in relatedEntities)
                {
                    AddEntityPermission(userRoleEntityData, entity);
                }
            }
            return userRoleEntityData;
        }

        private static void AddEntityPermission(UserRoleEntityData userRoleEntityData, RelatedEntity entity)
        {
            var jObj = JObject.Parse(entity.Object.ToString());
            var permission = new EntityPermission
            {
                Entity = jObj[Name].ToString(),
                Permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { Permissions.Admin } // hard coded so if you have permissions for an entity, you are an admin for now. Later, this value to come from configuration.
            };
            userRoleEntityData.GetOrAdd(permission.Entity, permission);
            userRoleEntityData.EntityIds.Add(entity.Id.ToInt(), permission.Entity);
        }
    }
}