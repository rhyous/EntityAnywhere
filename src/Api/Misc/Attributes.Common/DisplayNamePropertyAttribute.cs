using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// This attribute is used to hint to the UI which property to display
    /// when it can only display one property of an entity.
    /// </summary>
    /// <remarks>If this property is there, it should be used.
    /// If this property is not there, then Name should be used.
    /// If this property is not there and a Name property doesn't exist,
    /// then Id should be used.</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisplayNamePropertyAttribute : EntityAttribute
    {
        /// <summary>
        /// Enter the primary visual property name in this constructor.
        /// </summary>
        /// <param name="property"></param>
        public DisplayNamePropertyAttribute(string property)
        {
            Property = property;
        }

        /// <summary>
        /// The primary visual property name..
        /// </summary>
        public string Property
        {
            get { return _Property; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("The Property must be specified.", nameof(Property));
                _Property = value;
            }
        } private string _Property;
    }
}
