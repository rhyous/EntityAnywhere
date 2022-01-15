using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// The standard WebService interface is IWebServiceCommon{TEntity, TInterface, Tid}. It has three generic types.
    /// If a custom WebService is written with additional generic types and you want the plugin loader
    /// to find it, then this attribute must be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AdditionalWebServiceTypesAttribute : Attribute, IAdditionalTypes
    {
        /// <summary>
        /// When a WebService needs more generic Types than the default, specify them here.
        /// </summary>
        /// <param name="types">A list of Types to use as Generic Parameters.</param>
        public AdditionalWebServiceTypesAttribute(params Type[] additionalTypes) => AdditionalTypes = additionalTypes;

        /// <inheritdoc />
        public Type[] AdditionalTypes { get; set; }
    }
}