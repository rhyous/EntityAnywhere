using System;
using System.Runtime.InteropServices;

namespace Rhyous.WebFramework.Interfaces
{
    /// <summary>
    /// Not implemented or used yet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RelatedEntityAttribute : Attribute
    {

        public RelatedEntityAttribute(string entity, [Optional] string foreignKey, [Optional] Type foreignKeyType)
        {
            Entity = entity;
            ForeignKey = string.IsNullOrWhiteSpace(foreignKey) ? $"{Entity}Id" : foreignKey;
            foreignKeyType = foreignKeyType ?? typeof(int);
        }

        public string Entity { get; set; }

        public string ForeignKey { get; set; }

        public Type ForeignKeyType { get; set; }
    }
}
