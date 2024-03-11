using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    internal static class EntityExtensions
    {
        public static EntityWithMissingProperties ToEntityWithMissingProperties(this EntitySettings entitySettings, Type entityType)
        {
            var entityWithMissingProperties = new EntityWithMissingProperties
            {
                Entity = entitySettings.Entity,
                EntityGroup = entitySettings.EntityGroup,
            };
            var altKeyAttrib = entityType.GetAttribute<AlternateKeyAttribute>();
            if (altKeyAttrib != null)
                entityWithMissingProperties.SearchableProperties.Add(altKeyAttrib.KeyProperty);
            return entityWithMissingProperties;
        }
    }
}