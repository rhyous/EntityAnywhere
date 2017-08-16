using System;

namespace Rhyous.WebFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : Attribute
    {
        public PluralNameAttribute(string pluralName)
        {
            PluralName = pluralName;
        }

        public string PluralName
        {
            get { return _PluralName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Plural name must be specified.");
                _PluralName = value;
            }
        } private string _PluralName;
    }
}
