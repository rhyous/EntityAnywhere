using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    class EntityNameProvider : IEntityNameProvider
    {
        private const string EntityWebHost = nameof(EntityWebHost);
        private const string EntitySubpath = nameof(EntitySubpath);
        private const string LegacyServiceExtension = ".svc";

        private readonly IAppSettings _AppSettings;

        public EntityNameProvider(IAppSettings appSettings)
        {
            _AppSettings = appSettings;
        }
        public string Provide(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) { throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path)); }

            var SubPath = _AppSettings.Collection.Get(EntitySubpath)?.Trim('/');
            var startIndex = string.IsNullOrWhiteSpace(SubPath)
                           ? 1
                           : SubPath.Length + 2;
            var substring = path.Substring(startIndex);
            
            if (substring.Contains(LegacyServiceExtension))
                substring = substring.Replace(LegacyServiceExtension, "");

            var entityService = substring.Split('/').First();
            var entity = entityService.Substring(0, entityService.Length - "Service".Length);
            return entity;
        }
    }
}
