using System;

namespace Rhyous.WebFramework.Interfaces
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AlternateKeyAttribute : EntityAttribute
    {
        public AlternateKeyAttribute() { }
        public AlternateKeyAttribute(string idProperty) { KeyProperty = idProperty; }
        public string KeyProperty { get; set; }
    }
}
