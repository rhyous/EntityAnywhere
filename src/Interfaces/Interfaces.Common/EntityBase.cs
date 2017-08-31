using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// And abstract base class.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public abstract class EntityBase<TId> : IEntity<TId>
    {
        /// <inheritdoc />
        public TId Id { get; set; }
    }
}
