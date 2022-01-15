using Rhyous.Collections;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.HeaderValidators
{
    internal class PathNormalizer : IPathNormalizer
    {
        private readonly IAppSettings _AppSettings;
        private readonly HashSet<string> _SubPaths = new HashSet<string> { "/Api", "/EntitlementServices32", "/EntitlementServices" };

        public PathNormalizer(IAppSettings appSettings)
        {
            _AppSettings = appSettings;
        }
        public string Normalize(string path)
        {
            path = path.Replace(".svc", "");
            var entitySubPath = _AppSettings.Collection.Get("EntitySubpath", "");
            if (!string.IsNullOrWhiteSpace(entitySubPath))
                _SubPaths.Add($"/{entitySubPath}");
            foreach (var subPath in _SubPaths)
            {
                if (path.Contains($"{subPath}/", StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(subPath.Length);
                    break;
                }
            }
            return path;
        }
    }
}