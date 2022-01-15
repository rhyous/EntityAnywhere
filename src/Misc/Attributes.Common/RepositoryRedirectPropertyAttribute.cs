using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    public class RepositoryRedirectPropertyAttribute : Attribute
    {
        public RepositoryRedirectPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}