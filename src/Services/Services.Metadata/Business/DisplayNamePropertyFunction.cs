using Rhyous.Odata.Csdl;
using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services
{
    internal class DisplayNamePropertyFunction
    {
        private const string UIDisplayName = "@UI.DisplayName";

        /// <summary>
        /// This method is used to turn the DisplayNamePropertyAttribute into
        /// "@UI.DisplayName" metadata inside a CsdlProperty.
        /// </summary>
        /// <param name="mi">A method info.</param>
        /// <returns>one or more KeyValuePairs that can be added as Property.</returns>
        internal IEnumerable<KeyValuePair<string, object>> GetDisplayNameProperty(MemberInfo mi)
        {
            var attrib = mi?.GetCustomAttribute<DisplayNamePropertyAttribute>(true);
            if (attrib == null)
                return null;
            return new[]
            {
                new KeyValuePair<string, object>(UIDisplayName, new CsdlPropertyPath { PropertyPath = attrib.Property })
            };
        }

    }
}
