namespace Rhyous.WebFramework.Clients
{
    public interface IEntityClientBase
    {
        /// <summary>
        /// This is the url to the entity service. For example: https://host/path/to/api/Entity1Service.svc
        /// </summary>
        string ServiceUrl { get; set; }

        /// <summary>
        /// The entity name.
        /// </summary>
        string Entity { get; }
    }
}