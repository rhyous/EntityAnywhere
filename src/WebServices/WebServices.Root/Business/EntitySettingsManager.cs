using Autofac;
using Newtonsoft.Json;
using Rhyous.Collections;
using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class EntitySettingsManager
    {
        private const int MaxEntityGroups = 100; // The expectation is that Entity Groups will never reach 100.

        private readonly IEnumerable<Type> _LoadedEntities;
        private readonly IDictionary<Type, CsdlEntity> _EntityMetadata;
        private readonly IEntitySettingsProvider _EntitySettingsProvider;
        private readonly IEntityClientAsync<EntityGroup, int> _EntityGroupClient;
        private readonly IMissingEntitySettingDetector _MissingEntitySettingDetector;
        private readonly IEntitySettingsWriter _EntitySettingsWriter;
        private readonly ILogger _Logger;

        public EntitySettingsManager(IEnumerable<Type> loadedEntities, 
                                     IDictionary<Type,CsdlEntity> entityMetadata,
                                     IEntitySettingsProvider entitySettingsProvider,
                                     IEntityClientAsync<EntityGroup, int> entityGroupClient,
                                     IMissingEntitySettingDetector missingEntitySettingDetector,
                                     IEntitySettingsWriter entitySettingsWriter,
                                     ILogger logger) {
            _LoadedEntities = loadedEntities ?? throw new ArgumentNullException(nameof(loadedEntities));
            _EntityMetadata = entityMetadata ?? throw new ArgumentNullException(nameof(entityMetadata));
            _EntitySettingsProvider = entitySettingsProvider ?? throw new ArgumentNullException(nameof(entitySettingsProvider));
            _EntityGroupClient = entityGroupClient ?? throw new ArgumentNullException(nameof(entityGroupClient));
            _MissingEntitySettingDetector = missingEntitySettingDetector ?? throw new ArgumentNullException(nameof(missingEntitySettingDetector));
            _EntitySettingsWriter = entitySettingsWriter ?? throw new ArgumentNullException(nameof(entitySettingsWriter));
            _Logger = logger;
        }

        /// <summary>
        /// Load any missing entity settings
        /// </summary>
        /// <returns>Task</returns>
        public async Task<IDictionary<string, EntitySetting>> LoadDefaults()
        {
            _Logger.Debug("Loading Entity Settngs.");
            var entitySettings = (await _EntitySettingsProvider.GetAsync()) 
                               ?? new Dictionary<string, EntitySetting>();
            var missing = _MissingEntitySettingDetector.Detect(entitySettings, _LoadedEntities);
            if (!missing.Any())
            {
                _Logger.Debug("All entity settings were already loaded.");
                return entitySettings;
            }
            _Logger.Debug("All entity settings were already loaded.");
            await _EntitySettingsWriter.Write(missing);
            return await _EntitySettingsProvider.GetAsync();
        }


        public static async Task<EntitySettingsManager> Factory(
            IComponentContext context,
            IEntityList entityList,
            IMetadataCache metadataCache,
            ILogger logger
        )
        {
            var jsonSettings = GetJsonSettings();
            var entityClient = context.Resolve<IAdminEntityClientAsync<Entity, int>>();
            var entitySettingsProvider = new EntitySettingsProvider(entityClient);
            var entityGroupClient = context.Resolve<IAdminEntityClientAsync<EntityGroup, int>>();
            var existingGroups = await GetGroups(entityGroupClient);
            var missingEntitySettingDetector = new MissingEntitySettingDetector(existingGroups);
            var entityPropertyClient = context.Resolve<IAdminEntityClientAsync<EntityProperty, int>>();
            var entitySettingsWriter = new EntitySettingsWriter(entityClient, entityPropertyClient, entityGroupClient, logger);
            return new EntitySettingsManager(entityList.Entities,
                                             metadataCache.EntityMetadata,
                                             entitySettingsProvider,
                                             entityGroupClient,
                                             missingEntitySettingDetector,
                                             entitySettingsWriter,
                                             logger
                                             );
        }

        private static JsonSerializerSettings GetJsonSettings()
        {
            var jsonResolver = new IgnorePropertiesContractResolver();
            jsonResolver.Ignore("Id", "CreateDate", "CreatedBy", "LastUpdated", "LastUpdatedBy");
            return new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                ContractResolver = jsonResolver
            };
        }

        internal static async Task<Dictionary<string, EntityGroup>> GetGroups(IEntityClientAsync<EntityGroup, int> entityGroupClient)
        {
            var odataGroups = await entityGroupClient.GetByQueryParametersAsync($"$top={MaxEntityGroups}");
            var dictionary = new Dictionary<string, EntityGroup>();
            var existingGroupsKvps = (odataGroups)?.Select(o => o.Object)
                                                   .Select(g => new KeyValuePair<string, EntityGroup>(g.Name, g));
            if (existingGroupsKvps != null && existingGroupsKvps.Any())
                dictionary.AddRange(existingGroupsKvps);
            return dictionary;
        }
    }
}