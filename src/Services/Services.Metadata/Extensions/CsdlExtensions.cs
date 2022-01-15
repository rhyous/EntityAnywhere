using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary.Pluralization;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services
{
    public static class CsdlExtensions
    {
        public static void AddAlternateKey(this CsdlEntity csdl, Type entityType)
        {
            var attrib = entityType.GetAttribute<AlternateKeyAttribute>(true);
            if (attrib != null)
                csdl.Keys.Add(attrib.KeyProperty);
        }

        public static void AddExtensionEntityNavigationProperties(this CsdlEntity csdl, Type entityType, IEnumerable<Type> extensionEntities)
        {
            var extensionEntitiesToUse = extensionEntities.ToList();
            var entityExclusions = entityType.GetAttribute<RelatedEntityExclusionsAttribute>();
            if (entityExclusions != null && entityExclusions.Exclusions != null && entityExclusions.Exclusions.Any())
            {
                if (entityExclusions.Exclusions.Any(e => e == "*"))
                    extensionEntitiesToUse.Clear();
                else
                    extensionEntitiesToUse = extensionEntitiesToUse.Where(e=> !entityExclusions.Exclusions.Contains(e.Name)).ToList();
            }
            foreach (var extensionEntity in extensionEntitiesToUse)
            {
                var navProp = new CsdlNavigationProperty
                {
                    Type = $"self.{extensionEntity.Name}",
                    IsCollection = true, // Extension entities are always a collection.
                    Nullable = true    // Collections can always be empty}
                };
                navProp.CustomData.Add("@EAF.RelatedEntity.Type", "Extension");
                csdl.Properties.Add(extensionEntity.Name.Pluralize(), navProp);
            }
        }
    }
}
