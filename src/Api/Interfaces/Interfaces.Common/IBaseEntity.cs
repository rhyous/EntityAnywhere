namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The base interface for all entities. This is the Interface that the plugin loader uses to determine if a class is an entity.
    /// However, this should not be applied directly to an Entity. Use IEntity{Tid} instead.
    /// </summary>
    public interface IBaseEntity
    {
    }

    /// <summary>
    /// The generic base interface for all entities. Every entity must implement this.
    /// </summary>
    /// <typeparam name="TId">The type of the Id property. Usually int, long, guid, string, etc...</typeparam>
    public interface IBaseEntity<TId> : IBaseEntity, IId<TId>
    {
    }
}
