namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// This enum is used for ServiceBehavior plugins. By giving it a type, Entity Web Services or Custom Web Services can properly include or exclude them with one fo the following attributes:
    /// - ExcludedServiceBehaviorTypesAttribute
    /// - IncludedServiceBehaviorTypesAttribute
    /// </summary>
    public enum ServiceBehaviorType
    {
        /// <summary>None service behavior type.</summary>
        None,
        /// <summary>Authenticator service behavior type.</summary>
        /// <remarks>This must be excluded by authentication services. 
        /// The system cannot require authentication from a user that
        /// has yet to authenticate.</remarks>
        Authenticator,
        /// <summary>Logger service behavior type.</summary>
        Logger,
        /// <summary>ErrorHandler service behavior type.</summary>
        ErrorHandler,
        /// <summary>Other service behavior type.</summary>
        Other
    }
}
