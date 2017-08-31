using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// The standard web service interface is IEntityWebService{TEntity, TInterface, Tid, TService}. It has four generic types.
    /// If a custom service is written with additional generic types, this attribute must be used.
    /// </summary>
    public class AdditionalWebServiceTypesAttribute : Attribute, IAdditionalTypes
    {
        public AdditionalWebServiceTypesAttribute(params Type[] types) { _Types = types.ToList(); }

        /// <inheritdoc />
        public List<Type> Types
        {
            get { return _Types ?? (_Types = new List<Type>()); }
            set { _Types = value; }
        } private List<Type> _Types;
    }
}
