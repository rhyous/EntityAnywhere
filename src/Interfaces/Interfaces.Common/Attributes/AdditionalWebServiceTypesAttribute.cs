using System;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The standard web service interface is IEntityWebService{TEntity, TInterface, Tid, TService}. It has four generic types.
    /// If a custom service is written with additional generic types, this attribute must be used.
    /// </summary>
    public class AdditionalWebServiceTypesAttribute : Attribute, IAdditionalTypes
    {
        public AdditionalWebServiceTypesAttribute(params Type[] types) { Types = types; }

        /// <inheritdoc />
        public Type[] Types { get; set; }
    }
}
