using System.ComponentModel.DataAnnotations;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// And abstract base class.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class Entity<TId> : IEntity<TId>
    {
        /// <inheritdoc />
        [Key]
        public virtual TId Id { get; set; }
    }
}