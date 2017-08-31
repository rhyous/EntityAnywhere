using System;

namespace Rhyous.WebFramework.Interfaces.Attributes
{
    /// <summary>
    /// Not implemented or used yet.
    /// </summary>
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
