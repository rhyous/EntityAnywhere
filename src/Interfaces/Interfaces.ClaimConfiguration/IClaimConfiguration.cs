using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// A configuration that tells how claims can be built from Entities dynamically.
    /// </summary>
    /// <remarks>Future: This will  likely become its own Entity</remarks>
    public interface IClaimConfiguration : IEntity<int>, IName, IAuditable
    {
        /// <summary>
        /// The name of the claims domain this claim is for. This becomes the Subject of the ClaimDomain. This entity
        /// must be eitherUser, a RelatedEntity to User.
        /// </summary>
        /// <remarks>Future: Support a RelatedEntity of a User's RelatedEntity recursively.</remarks>
        string Domain { get; set; }
        /// <summary>
        /// The entity this claim is for. If Domain is null, this becomes the Subject of the ClaimDomain. This entity
        /// must be eitherUser, a RelatedEntity to User.
        /// </summary>
        /// <remarks>Future: Support a RelatedEntity of a User's RelatedEntity recursively.</remarks>
        string Entity { get; set; }
        /// <summary>
        /// The Property on the Entity from which the claim value is pulled.
        /// </summary>
        string EntityProperty { get; set; }
        /// <summary>
        /// The Property on either User or the first RelatedEntity that hold the Id.
        /// </summary>
        string EntityIdProperty { get; set; }
        /// <summary>
        /// The Property on the User entity that relates to a RelatedEntity.
        /// </summary>
        string RelatedEntityIdProperty { get; set; }
    }
}
