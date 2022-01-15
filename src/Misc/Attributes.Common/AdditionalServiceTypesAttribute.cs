using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// The standard service interface is IServiceCommon{TEntity, TInterface, Tid}. It has three generic types.
    /// If a custom service is written with additional generic types and you want the plugin loader
    /// to find it, then this attribute must be used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AdditionalServiceTypesAttribute : Attribute, IAdditionalTypes
    {
        /// <summary>
        /// When a service needs more generic Types than the default, specify them here.
        /// </summary>
        /// <param name="types">A list of Types to use as Generic Parameters.</param>
        public AdditionalServiceTypesAttribute(params Type[] additionalTypes) => AdditionalTypes = additionalTypes;
        /// <inheritdoc />
        public Type[] AdditionalTypes { get; set; }
    }
}