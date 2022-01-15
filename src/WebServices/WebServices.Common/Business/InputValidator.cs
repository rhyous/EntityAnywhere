using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class InputValidator<TEntity, TId> : IInputValidator<TEntity, TId>
        where TEntity : IId<TId>
    {
        private readonly IEntityInfo<TEntity> _EntityInfo;

        public InputValidator(IEntityInfo<TEntity> entityInfo)
        {
            _EntityInfo = entityInfo;
        }

        /// <summary>
        /// Clean and Validate for UpdatePropertyHandler
        /// </summary>
        /// <param name="entityType">The Entity type</param>
        /// <param name="id">The entity Id.</param>
        /// <param name="property">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <returns>True if cleaned and validated, false otherwise.</returns>
        public bool CleanAndValidate(Type entityType, ref string id, ref string property, ref string value)
        {
            if (entityType == null || string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(property))
                return false;
            id = id.TrimAll();
            property = property.TrimAll();
            if (!_EntityInfo.Properties.TryGetValue(property, out PropertyInfo pi))
                throw new RestException($"Entity '{typeof(TEntity).Name}' does not have a '{property}' property.", HttpStatusCode.BadRequest);
            if (!pi.IgnoreTrim())
                value = value?.TrimAll();
            if (id.Equals(default(TId)))
                return false;
            return true;
        }

        /// <summary>
        /// Clean and Validate for PatchHandler
        /// </summary>
        /// <param name="id">The entity Id.</param>
        /// <param name="patchedEntity">The patched entity</param>
        /// <returns></returns>
        public bool CleanAndValidate(ref string id, PatchedEntity<TEntity, TId> patchedEntity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;
            if (patchedEntity == null || patchedEntity.Entity == null || patchedEntity.ChangedProperties == null)
                return false;
            patchedEntity.Entity.TrimStringProperties();
            id = id.TrimAll();
            var typedId = id.To(typeof(TId));
            if (typedId.Equals(default(TId)))
                return false;
            return true;
        }

        /// <summary>
        /// Clean and Validate for PatchHandler
        /// </summary>
        /// <param name="id">The entity Id.</param>
        /// <param name="patchedEntity">The patched entity</param>
        /// <returns></returns>
        public bool CleanAndValidate(PatchedEntityCollection<TEntity, TId> patchedEntityCollection)
        {
            if (patchedEntityCollection == null
             || patchedEntityCollection.PatchedEntities == null
             || !patchedEntityCollection.PatchedEntities.Any()
             || patchedEntityCollection.PatchedEntities.Any(pe => pe.Entity == null))
                return false;
            bool hasGlobalPropertyChanges = patchedEntityCollection.ChangedProperties != null && patchedEntityCollection.ChangedProperties.Any();
            foreach (var pe in patchedEntityCollection.PatchedEntities)
            {
                if (pe.Entity.Id.Equals(default(TId)))
                    return false; // Every one must have an Id. We need the id to know which entity to patch.
                pe.Entity.TrimStringProperties();
                if (hasGlobalPropertyChanges)
                {
                    if (pe.ChangedProperties == null)
                        pe.ChangedProperties = patchedEntityCollection.ChangedProperties.ToHashSet(); //ToList() will clone it
                    else
                    {
                        foreach (var prop in patchedEntityCollection.ChangedProperties)
                            pe.ChangedProperties.Add(prop);
                    }
                }
                if (pe.ChangedProperties == null || !pe.ChangedProperties.Any())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Clean and Validate for PutHandler
        /// </summary>
        /// <param name="id">The entity Id.</param>
        /// <param name="entity">The entity to put.</param>
        /// <returns></returns>
        public bool CleanAndValidate(ref string id, TEntity entity)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;
            if (entity == null)
                return false;
            id = id.TrimAll();
            var typedId = id.To(typeof(TId));
            if (typedId.Equals(default(TId)))
                return false;
            entity.TrimStringProperties();
            return true;
        }
    }
}