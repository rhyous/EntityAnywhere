using Rhyous.Collections;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class EntityClientConfig : IEntityClientConfig
    {
        private readonly IAppSettings _AppSettings;

        public EntityClientConfig(IAppSettings appSettings)
        {
            _AppSettings = appSettings;
        }

        public string EntityAdminToken => _AppSettings.Collection.Get("EntityAdminToken", "");

        public string EntityWebHost => _AppSettings.Collection.Get("EntityWebHost", "");

        public string EntitySubpath => _AppSettings.Collection.Get("EntitySubpath", "");
    }
}