namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// The CustomWebServiceAttribute. This must be used by all custom web services plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CustomControllerAttribute : Attribute
    {
        /// <summary>
        /// An attribute for a custom service.
        /// </summary>
        /// <param name="entity">The name of an entity this custom web service is for or null if not for a entity.</param>
        public CustomControllerAttribute(Type? entity = null)
        {
            Entity = entity;
        }
        /// <summary>
        /// If the custom webservice is for an Entity, specify what entity it is for.
        /// Otherwise, leave this blank or don't use this attribute.
        /// </summary>
        public Type? Entity { get; set; }
    }
}