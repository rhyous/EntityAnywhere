using Rhyous.EntityAnywhere.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// And abstract base class.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class BaseEntity<TId> : IBaseEntity<TId>
    {
        /// <inheritdoc />
        [Key]
        public virtual TId Id { get; set; }
    }
}