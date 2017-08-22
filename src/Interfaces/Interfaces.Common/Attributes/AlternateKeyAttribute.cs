using System;

namespace Rhyous.WebFramework.Interfaces
{
    public class AlternateKeyAttribute : Attribute
    {
        public AlternateKeyAttribute() { }
        public AlternateKeyAttribute(string idProperty) { KeyProperty = idProperty; }
        public string KeyProperty { get; set; }
    }
}
