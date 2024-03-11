using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// The interface is used to enforce that AdditionalWebServiceTypesAttribute and AdditionalServiceTypesAttribute have a common interface.
    /// </summary>
    public interface IAdditionalTypes
    {
        /// <summary>
        /// The additional generic types for implementing a child of ServicesCommon{,,}.
        /// </summary>
        Type[] AdditionalTypes { get; set; }
    }
}