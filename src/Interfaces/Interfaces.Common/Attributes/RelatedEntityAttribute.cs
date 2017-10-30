using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// Not implemented or used yet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RelatedEntityAttribute : Attribute, IRelatedEntity
    {
        public const string DefaultForeignKey = "Id";

        public RelatedEntityAttribute(string entity, [Optional] string foreignKey, [Optional] Type foreignKeyType, [Optional] bool autoExpand, [CallerMemberName]string property = null)
        {
            RelatedEntity = entity;
            ForeignKey = string.IsNullOrWhiteSpace(foreignKey) ? DefaultForeignKey : foreignKey;
            foreignKeyType = foreignKeyType ?? typeof(int);
            AutoExpand = autoExpand;
            Property = property;
        }

        /// <summary>
        /// The name of the related entity.
        /// </summary>
        public string RelatedEntity { get; set; }

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
        /// The name of the Property this attribute decorates.
        /// </summary>
        public string Property { get; set; }
    }
}
