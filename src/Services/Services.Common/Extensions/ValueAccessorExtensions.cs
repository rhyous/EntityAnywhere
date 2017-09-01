using System;
using System.Linq;
using System.Reflection;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// This provides simple methods to get values of any object instance using reflection.
    /// </summary>
    public static class ValueAccessorExtensions
    {
        public static StringComparison Comparison { get; set; } = StringComparison.InvariantCultureIgnoreCase;

        /// <summary>
        /// A static method to get the PropertyInfo of a property of any object.
        /// </summary>
        /// <param name="type">The Type that has the property</param>
        /// <param name="propertyName">The name of the  property</param>
        /// <returns>PropertyInfo object. It has the property name and a useful GetValue() method.</returns>
        public static PropertyInfo GetPropertyInfo(this object o, string propertyName)
        {
            return o.GetType().GetPropertyInfo(propertyName);
        }

        /// <summary>
        /// A static method to get the PropertyInfo of a property of any object.
        /// </summary>
        /// <param name="type">The Type that has the property</param>
        /// <param name="propertyName">The name of the  property</param>
        /// <returns>PropertyInfo object. It has the property name and a useful GetValue() method.</returns>
        public static PropertyInfo GetPropertyInfo(this Type t, string propertyName)
        {
            var props = t.GetProperties();
            return props.FirstOrDefault(propInfo => propInfo.Name.Equals(propertyName, Comparison));
        }

        /// <summary>
        /// A static method to get the value of a property of any object.
        /// </summary>
        /// <param name="o">The instance from which to read the value.</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property boxed as an object.</returns>
        public static object GetPropertyValue(this object o, string propertyName, object defaultValue = null)
        {
            return o.GetType().GetPropertyInfo(propertyName)?.GetValue(o) ?? defaultValue;
        }

        /// <summary>
        /// A static method to get the value of a property of any object.
        /// </summary>
        /// <param name="o">The instance from which to read the value.</param>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>The value of the property boxed as an object.</returns>
        public static object GetStaticPropertyValue(this Type t, string propertyName, object defaultValue = null)
        {
            var tmpType = t;
            object obj = null;
            int count = 0;
            while (obj == null && tmpType != null && count < 10)
            {
                obj = tmpType.GetProperty("EntityType")?.GetValue(null);
                tmpType = tmpType.BaseType;
                count++;
            }
            return obj ?? defaultValue;
        }

        /// <summary>
        /// A static method to get the FieldInfo of a field of any object.
        /// </summary>
        /// <param name="type">The Type that has the field</param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>FieldInfo object. It has the field name and a useful GetValue() method.</returns>
        public static FieldInfo GetFieldInfo(this object o, string fieldName)
        {
            return o.GetType().GetFieldInfo(fieldName);
        }

        /// <summary>
        /// A static method to get the FieldInfo of a field of any object.
        /// </summary>
        /// <param name="type">The Type that has the field</param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>FieldInfo object. It has the field name and a useful GetValue() method.</returns>
        public static FieldInfo GetFieldInfo(this Type t, string fieldName)
        {
            var fields = t.GetFields();
            return fields.FirstOrDefault(fieldInfo => fieldInfo.Name.Equals(fieldName, Comparison));
        }

        /// <summary>
        /// A static method to get the FieldInfo of a field of any object.
        /// </summary>
        /// <param name="o">The instance from which to read the value.</param>
        /// <param name="fieldName">The name of the field</param>
        /// <returns>The value of the property boxed as an object.</returns>
        public static object GetFieldValue(this object o, string fieldName, object defaultValue = null)
        {
            return GetFieldInfo(o.GetType(), fieldName)?.GetValue(o) ?? defaultValue;
        }
    }
}