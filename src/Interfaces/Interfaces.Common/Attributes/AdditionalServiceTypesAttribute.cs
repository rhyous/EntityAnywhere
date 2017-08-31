using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The standard service interface is IServiceCommon{TEntity, TInterface, Tid}. It has three generic types.
    /// If a custom service is written with additional generic types and you want the plugin loader
    /// to find it, then this attribute must be used.
    /// </summary>
    public class AdditionalServiceTypesAttribute : Attribute, IAdditionalTypes
    {
        public AdditionalServiceTypesAttribute(params Type[] types) { }

        /// <inheritdoc />
        public List<Type> Types
        {
            get { return _Types ?? (_Types = new List<Type>()); }
            set { _Types = value; }
        } private List<Type> _Types;
    }
}
