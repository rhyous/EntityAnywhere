using System;

namespace Rhyous.WebFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : Attribute
    {
        public PluralNameAttribute(string pluralName)
        {
            _PluralName = pluralName;
        }

        public string PluralName => _PluralName;
        private string _PluralName;
    }
}
