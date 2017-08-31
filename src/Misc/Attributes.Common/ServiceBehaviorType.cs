namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// This enum is used for ServiceBehavior plugins. By giving it a type, Entity Web Services or Custom Web Services can properly include or exclude them with one fo the following attributes:
    /// - ExcludedServiceBehaviorTypesAttribute
    /// - IncludedServiceBehaviorTypesAttribute
    /// </summary>
    public enum ServiceBehaviorType
    {
        None,
        Authenticator,
        Logger,
        ErrorHandler,
        Other
    }
}
