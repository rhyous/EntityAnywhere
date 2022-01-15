using Autofac;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class EntitySettingsHandler : IEntitySettingsHandler
    {
        private readonly IComponentContext _Context;
        private readonly IEntityList _EntityList;
        private readonly IMetadataCache _MetadataCache;
        private readonly ILogger _Logger;

        public EntitySettingsHandler(
            IComponentContext context,
            IEntityList entityList,
            IMetadataCache metadataCache,
            ILogger logger
        )
        {
            _Context = context;
            _EntityList = entityList ?? throw new ArgumentNullException(nameof(entityList));
            _MetadataCache = metadataCache ?? throw new ArgumentNullException(nameof(metadataCache));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Dictionary<string, EntitySetting>> Handle()
        {
            var settingsMgr = await EntitySettingsManager.Factory(_Context,
			                                                      _EntityList,
                                                                  _MetadataCache,
                                                                  _Logger);
            return await settingsMgr.LoadDefaults() as Dictionary<string, EntitySetting>;
        }
    }
}