using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Exceptions;
using System;

namespace Rhyous.EntityAnywhere.Clients2
{
    public static class EntityClientConfigExtensions
    {
        public static string GetUrl(this IEntityClientConfig entityClientConfig, string customSubpath = null)
        {
            if (entityClientConfig is null) { throw new ArgumentNullException(nameof(entityClientConfig)); }

            if (string.IsNullOrWhiteSpace(entityClientConfig.EntityWebHost))
                throw new MissingConfigurationException($"The {entityClientConfig.EntityWebHost} must be configured.");

            var subPath = string.IsNullOrWhiteSpace(customSubpath) ? entityClientConfig.EntitySubpath : customSubpath;
            return string.IsNullOrWhiteSpace(subPath)
                   ? StringConcat.WithSeparator('/', entityClientConfig.EntityWebHost)
                   : StringConcat.WithSeparator('/', entityClientConfig.EntityWebHost, subPath);
        }

        public static string GetServiceUrl(this IEntityClientConfig entityClientConfig, string service)
        {
            if (string.IsNullOrWhiteSpace(service)) { throw new ArgumentException($"'{nameof(service)}' cannot be null or whitespace.", nameof(service)); }

            return StringConcat.WithSeparator('/', entityClientConfig.GetUrl(), service);
        }
    }
}