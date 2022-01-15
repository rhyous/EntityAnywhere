using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This caches User Role to Entity mappings for entity Authorization.
    /// </summary>
    /// <remarks>Interfaces.Authentication cannot reference the Entities.User plugin, so Json client is used.</remarks>
    public class UserRoleEntityDataCacheFactory : IUserRoleEntityDataCacheFactory
    {
        internal const string Entity = nameof(Entity);
        public const string Expand = "$Expand=Entity";
        internal const string Name = nameof(IName.Name);
        internal const string UserRole = nameof(UserRole);
        private readonly INamedFactory<IAdminEntityClientAsync> _NamedFactory;
        private readonly IPreventSimultaneousFuncCalls<Task<IUserRoleEntityDataCache>> _PreventSimultaneousFuncCalls;

        public UserRoleEntityDataCacheFactory(INamedFactory<IAdminEntityClientAsync> namedFactory, 
                                              IPreventSimultaneousFuncCalls<Task<IUserRoleEntityDataCache>> preventSimultaneousFuncCalls)
        {
            _NamedFactory = namedFactory;
            _PreventSimultaneousFuncCalls = preventSimultaneousFuncCalls;
        }

        public IUserRoleEntityDataCache Cache
        {
            get { return _Cache ?? (_Cache = _PreventSimultaneousFuncCalls.Call(CreateAsync).Result); }
            set { _Cache = value; }
        } private IUserRoleEntityDataCache _Cache;

        internal async Task<IUserRoleEntityDataCache> CreateAsync()
        {
            var client = _NamedFactory.Create(UserRole);
            var userRolesJson = await client.GetByQueryParametersAsync(Expand);
            var cache = new UserRoleEntityDataCache();
            if (string.IsNullOrWhiteSpace(userRolesJson) || (!userRolesJson.StartsWith("{") || !userRolesJson.EndsWith("}")))
                return cache;
            var odataObjectCollection = JsonConvert.DeserializeObject<OdataObjectCollection>(userRolesJson);
            foreach (var odataObject in odataObjectCollection)
            {
                var userRoleEntityData = CreateRoleEntityDataCache(odataObject);
                cache.GetOrAdd(userRoleEntityData.UserRoleId, userRoleEntityData);
                cache.UserRoleIds.Add(userRoleEntityData.UserRoleName, userRoleEntityData.UserRoleId);
            }
            return cache;
        }

        public async Task UpdateRoleEntityDataAsync(int roleId)
        {
            var client = _NamedFactory.Create(UserRole);
            var userRoleJson = await client.GetAsync(roleId.ToString(), Expand);
            var odataObject = JsonConvert.DeserializeObject<OdataObject>(userRoleJson);
            var userRoleEntityData = CreateRoleEntityDataCache(odataObject);
            Cache.Remove(roleId);
            Cache.UserRoleIds.Remove(userRoleEntityData.UserRoleName);
            Cache.Add(roleId, userRoleEntityData);
            Cache.UserRoleIds.Add(userRoleEntityData.UserRoleName, userRoleEntityData.UserRoleId);
        }

        private IUserRoleEntityData CreateRoleEntityDataCache(OdataObject odataUserRole)
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