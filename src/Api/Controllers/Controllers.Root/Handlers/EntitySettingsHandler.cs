using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class EntitySettingsHandler : IEntitySettingsHandler
    {
        private readonly IEntityList _EntityList;
        private readonly IEntitySettingsCache _EntitySettingsCache;
        private readonly IEntityGroupCache _EntityGroupCache;
        private readonly IMissingEntitySettingDetector _MissingEntitySettingDetector;
        private readonly IEntitySettingsWriter _EntitySettingsWriter;
        private readonly IUrlParameters _UrlParameters;
        private readonly ILogger _Logger;

        public EntitySettingsHandler(IEntityList entityList,
                                     IEntitySettingsCache entitySettingsCache,
                                     IEntityGroupCache entityGroupCache,
                                     IMissingEntitySettingDetector missingEntitySettingDetector,
                                     IEntitySettingsWriter entitySettingsWriter,
                                     IUrlParameters urlParameters,
                                     ILogger logger)
        {
            _EntityList = entityList;
            _EntitySettingsCache = entitySettingsCache;
            _EntityGroupCache = entityGroupCache;
            _MissingEntitySettingDetector = missingEntitySettingDetector;
            _EntitySettingsWriter = entitySettingsWriter;
            _UrlParameters = urlParameters;
            _Logger = logger;
        }

        public async Task<EntitySettingsDictionaryDto> Handle()
        {
            _Logger.Debug("Loading Entity Settings.");
            var forceUpdate = _UrlParameters.Collection.Get("ForceUpdate", false);
            var entitySettingsDictionary = await _EntitySettingsCache.ProvideAsync(forceUpdate);
            var entityGroupCache = await _EntityGroupCache.ProvideAsync(forceUpdate);
            var missing = await _MissingEntitySettingDetector.DetectAsync(entitySettingsDictionary, _EntityList.Entities);
            if (missing.Any())
            {
                _Logger.Debug("Some entity settings were missing in the repository. Writing them.");
                await _EntitySettingsWriter.Write(missing);
            }
            _Logger.Debug("All entity settings were already loaded.");
            return entitySettingsDictionary.ToDto();
        }
    }
}