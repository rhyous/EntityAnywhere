using System;

namespace Rhyous.WebFramework.Interfaces
{
    public class AlternateIdAttribute : Attribute
    {
        public AlternateIdAttribute() { }
        public AlternateIdAttribute(string idProperty) { IdProperty = idProperty; }
        public string IdProperty { get; set; }
    }
}
