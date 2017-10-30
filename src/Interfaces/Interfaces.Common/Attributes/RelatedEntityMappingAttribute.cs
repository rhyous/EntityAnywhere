using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// Not implemented or used yet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RelatedEntityMappingAttribute : Attribute, IRelatedEntity
    {
        public const string DefaultForeignKey = "Id";
        public const string DefaultMappingKey = "{0}Id";

        public RelatedEntityMappingAttribute(string relatedEntity, string mappingEntity, string entity, [Optional] string mappingKey, [Optional] string mappingForeignKey, [Optional] string foreignKey, [Optional] Type foreignKeyType, [Optional] bool autoExpand)
        {
            MappingEntity = mappingEntity;
            RelatedEntity = relatedEntity;
            MappingKey = mappingKey ?? string.Format(DefaultMappingKey, entity);
            MappingForeignKey = mappingForeignKey ?? string.Format(DefaultMappingKey, entity);
            ForeignKey = string.IsNullOrWhiteSpace(foreignKey) ? DefaultForeignKey : foreignKey;
            foreignKeyType = foreignKeyType ?? typeof(int);
            AutoExpand = autoExpand;
            Entity = entity;
        }
        /// <summary>
        /// This entity the attribute is applied to.
        /// </summary>
        public string Entity { get; set; }

        /// <summary>
        /// The name of the related entity.
        /// </summary>
        public string RelatedEntity { get; set; }

        /// <summary>
        /// The name of the mapping entity.
        /// </summary>
        public string MappingEntity { get; set; }
        /// <summary>
        /// The property in the mapping table tat references the related entity.
        /// </summary>
        public string MappingForeignKey { get; set; }

        /// <summary>
        /// The property in the mapping table that references this entity.
        /// </summary>
        public string MappingKey { get; set; }

        /// <summary>
        /// The name of the property identifier on the related entity.
        /// This key property is usually "Id", but could be an AlternateKey property.
        /// </summary>
        public string ForeignKey { get; set; }

        /// <summary>
        /// The type of the property identifier on the related entity.
        /// This type is usually the type of the key property "Id", but could be an AlternateKey property's type.
        /// </summary>
        public Type ForeignKeyType { get; set; }

        /// <summary>
        /// If this is true, a related entity will autoexpand. If false, it will only expand if the $expand Url parameters
        /// is used. If $expand is used, AutoExpand is ignored, so even AutoExpand related entities must be specified.
        /// is passed to the web service.
        /// </summary>
        public bool AutoExpand { get; set; }

        /// <summary>
        /// This is used for mapping entities with small numbers. It is faster to get all once.Also, this is only used when 
        /// returning a OdataObjectCollection. When this value is true, related entities will be included in the collection
        /// of related entities in the OdataObjectCollection, and will not be nested. This results in much smaller json
        /// serialization. 
        /// </summary>
        /// <remarks>This is ignored if getting a single entity.</remarks>
        /// <example>Imagine a UserType entity with only 10 total types. If a query returned 100 users with N UserTypes, then if
        /// the UserType was nested under each user, the json would include 100 * N instances of a related UserType in the json.</example>
        public bool GetAll { get; set; }
    }
}
