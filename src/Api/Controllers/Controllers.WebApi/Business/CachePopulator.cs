using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebApi
{
    internal class CachePopulator : ICachePopulator
    {
        private readonly IUserRoleEntityDataCache _UserRoleEntityDataCache;
        private readonly IEntitySettingsCache _EntitySettingsCache;

        public CachePopulator(IUserRoleEntityDataCache userRoleEntityDataCache,
                              IEntitySettingsCache entitySettingsCache)
        {
            _UserRoleEntityDataCache = userRoleEntityDataCache;
            _EntitySettingsCache = entitySettingsCache;
        }

        public async Task PopulateAsync()
        {
            var userRoletask = _UserRoleEntityDataCache.ProvideAsync();
            var entitySettingsTask = _EntitySettingsCache.ProvideAsync();
            await Task.WhenAll(userRoletask, entitySettingsTask);
        }
    }
}