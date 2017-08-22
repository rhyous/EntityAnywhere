using System;

namespace Rhyous.WebFramework.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RelatedEntityAttribute : Attribute
    {
        public string Name { get; set; }

        public RelatedEntityAttribute(string name)
        {
            Name = name;
        }
    }
}
