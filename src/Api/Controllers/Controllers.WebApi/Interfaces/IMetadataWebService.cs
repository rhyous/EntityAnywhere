namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// The service contract for the top level $Metadata service.
    /// </summary>
    public interface IMetadataWebService<T>
    {
        /// <summary>
        /// Gets the top level $Metadata for all services.
        /// </summary>
        /// <returns></returns>
        Task<T> GetMetadataAsync();
    }
}