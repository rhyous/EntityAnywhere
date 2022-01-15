namespace Rhyous.EntityAnywhere.WebServices
{
    public enum IdType { Id, Alt }

    public class DisambiguatedId<TId> : DisambiguatedId<TId, string> { };

    public class DisambiguatedId<TId, TAltKey>
    {
        /// <summary>
        /// The type: $Id or $Alt
        /// </summary>
        public IdType IdType { get; set; }
        /// <summary>
        /// The original id. For example: $id.12345
        /// </summary>
        public string OrginalId { get; set; }

        /// <summary>
        /// The Id after casting it to the generic TId type. If casting results in default(TId),
        /// then the Id provided is not an entity id. It must either be an Alternate Key or an
        /// Alternate Id.
        /// </summary>
        public TId Id { get; set; }

        /// <summary>
        /// The Id as a string.
        /// </summary>
        public TAltKey AltId { get; set; }

        /// <summary>
        /// This should be the property name of an alternate id.
        /// If the value is "$Key" (case insensitive) and the entity has the AlternateKeyAttribute
        /// then the alternate key should be used.
        /// </summary>
        public string AlternateIdProperty { get; set; }
    }
}