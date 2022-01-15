using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// This attribute must be added to any Entity that is a mapping Entity.
    /// A mapping entity maps two entities using many to many.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappingEntityAttribute : EntityAttribute
    {
        /// <summary>
        /// Whichever entity will have less instances should be Entity1. If this mapping entity maps and entity to an entity group, then entity1 should be the group.
        /// For example, for User, UserGroup, UserGroupMembership, Entity1 should be UserGroup.
        /// </summary>
        public string Entity1 { get; set; }

        /// <summary>
        /// If not specified, "Id" will be append to the value of Entity1.
        /// </summary>
        public string Entity1MappingProperty
        {
            get { return string.IsNullOrWhiteSpace(_Entity1MappingProperty) ? (_Entity1MappingProperty = $"{Entity1}Id") : _Entity1MappingProperty; }
            set { _Entity1MappingProperty = value; }
        } internal string _Entity1MappingProperty;

        /// <summary>
        /// Consider not set if null, empty, or whitespace. This allows for renaming one entity when both mappings are for the same entity.
        /// </summary>
        public string Entity1UriTemplate { get; set; }

        /// <summary>
        /// Whichever entity will have more instances should be Entity2. If this mapping entity maps and entity to an entity group, then entity2 should be the entity not the group.
        /// For example, for User, UserGroup, UserGroupMembership, Entity2 should be User.
        /// </summary>
        public string Entity2 { get; set; }

        /// <summary>
        /// If not specified, "Id" will be append to the value of Entity2.
        /// </summary>
        public string Entity2MappingProperty
        {
            get { return string.IsNullOrWhiteSpace(_Entity2MappingProperty) ? (_Entity2MappingProperty = $"{Entity2}Id") : _Entity2MappingProperty; }
            set { _Entity2MappingProperty = value; }
        } internal string _Entity2MappingProperty;

        /// <summary>
        /// Consider not set if null, empty, or whitespace. This allows for renaming one entity when both mappings are for the same entity.
        /// </summary>
        public string Entity2UriTemplate { get; set; }
    }
}
