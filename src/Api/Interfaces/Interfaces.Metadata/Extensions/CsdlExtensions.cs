using Rhyous.Odata;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary.Pluralization;
using Rhyous.EntityAnywhere.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public static class CsdlExtensions
    {
        public static void AddAlternateKey(this CsdlEntity csdl, Type entityType)
        {
            var attrib = entityType.GetAttribute<AlternateKeyAttribute>(true);
            if (attrib != null)
                csdl.Keys.Add(attrib.KeyProperty);
        }
        public static void AddFileUpload(this CsdlEntity csdl, Type entityType)
        {
            var fileUploadInterface = entityType.GetInterface(nameof(IFileUpload));
            if (fileUploadInterface != null)
                csdl.Properties.Add("@EAF.FileUpload", true);
        }

        public static void AddExtensionEntityNavigationProperties(this CsdlEntity csdl, Type entityType, IEnumerable<Type> extensionEntities)
        {
            // Don't AlternateId or Addendum on mapping entities
            if (entityType.GetAttribute<MappingEntityAttribute>() != null)
                return;
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
                navProp.CustomData.TryAdd("@EAF.RelatedEntity.Type", "Extension");
                csdl.Properties.TryAdd(extensionEntity.Name.Pluralize(), navProp);
            }
        }
    }
}
