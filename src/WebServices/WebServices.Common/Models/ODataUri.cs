using System;

namespace Rhyous.WebFramework.WebServices
{
    /// <summary>
    /// This object is used to map an Entity's property to a uri.
    /// </summary>
    public class ODataUri
    {
        /// <summary>
        /// The name of a property of an entity.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The web service uri to manage the property of an entity. 
        /// </summary>
        public Uri Uri { get; set; }
    }
}
