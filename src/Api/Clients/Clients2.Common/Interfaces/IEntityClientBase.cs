namespace Rhyous.EntityAnywhere.Clients2
{
    public partial interface IEntityClientBase
    {
        /// <summary>
        /// The entity name.
        /// </summary>
        string Entity { get; }
        
        /// <summary>
        /// The entity name pluralized.
        /// </summary>
        string EntityPluralized { get; }

        /// <summary>
        /// The url of the service, without a path or url paramaters.        /// 
        /// </summary>
        /// <example>https://www.domain.tld/Entity1Service</example>
        string ServiceUrl { get; }
    }
}