using System;

namespace Rhyous.WebFramework.Attributes
{
    /// <summary>
    /// The PluralName attribute. 
    /// Sometimes an Entity might exist that doesn't pluralize using -s or -es.
    /// This is useful as we don't support foriegn name Entities yet or foreign pluralization.
    /// There is also a PluralizationDictionary.cs in the Behaviors.RestEndpoint project where large lists would be easier to maintain.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluralNameAttribute : Attribute
    {
        public PluralNameAttribute(string pluralName)
        {
            PluralName = pluralName;
        }

        /// <summary>
        /// The plural name of the Entity.
        /// Examples: 
        /// Addenda (plural of Addendum)
        /// Fungi (plural of Fungus)
        /// </summary>
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
