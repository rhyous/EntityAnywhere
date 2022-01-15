using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// An entity to describe entity properties
    /// </summary>
    public class  EntityProperty : AuditableEntity<int>, IEntityProperty
    {
        /// <summary>
        /// The name of this Entity property
        /// </summary>
        [DistinctProperty("EntityProperty")]
        public string Name { get; set; }

        /// <summary>
        /// The description of this Entity property
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Type in code of this Entity property. Which is
        /// typeof(string).Name or System.String, not odata
        /// type which would be Edm.String.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The Id of the Entity where this property exists.
        /// </summary>
        [RelatedEntity("Entity")]
        [DistinctProperty("EntityProperty")]
        public int EntityId { get; set; }

        /// <summary>
        /// This is a sort order, for displaying properties.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// All properties can be searched, but this is for
        /// the UI to include it as a searchable option.
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Can this value be null?
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// IS it visable on a related entity?
        /// </summary>
        public bool VisibleOnRelations { get; set; }
    }
}