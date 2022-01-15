using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using Rhyous.EntityAnywhere.Exceptions;
using System;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public static class EntityClientConfigExtensions
    {
        public static string GetEntitlementServicesUrl(this ITestConfiguration testConfiguration, string customSubPath = null)
        {
            if (testConfiguration is null) { throw new ArgumentNullException(nameof(testConfiguration)); }

            if (string.IsNullOrWhiteSpace(testConfiguration.EntitlementServicesUrl))
                throw new MissingConfigurationException($"The {testConfiguration.EntitlementServicesUrl} must be configured.");

            var subPath = string.IsNullOrWhiteSpace(customSubPath)
                        ? testConfiguration.EntitlementServicesSubPath
                        : customSubPath;
            return string.IsNullOrWhiteSpace(subPath)
                   ? StringConcat.WithSeparator('/', testConfiguration.EntitlementServicesUrl)
                   : StringConcat.WithSeparator('/', testConfiguration.EntitlementServicesUrl, subPath);
        }
        public static string GetEntitlementServices32Url(this ITestConfiguration testConfiguration)
        {
            var subPath = string.IsNullOrWhiteSpace(testConfiguration.EntitlementServicesSubPath)
               ? null
               : $"{testConfiguration.EntitlementServicesSubPath}32";
            return testConfiguration.GetEntitlementServicesUrl(subPath);
        }
    }
}