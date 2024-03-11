namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// An interfaces for a software product entity.
    /// </summary>
    public interface IProduct : IBaseEntity<int>, IName, IDescription, IAuditable, IEnabled
    {
        /// <summary>
        /// The product type. 
        /// </summary>
        int TypeId { get; set; }
        /// <summary>
        /// Version is to go away in the future and be replaced by ProductRelease entities that have versions.
        /// </summary>
        string Version { get; set; }
    }
}
