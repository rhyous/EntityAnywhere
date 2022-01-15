using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// Use this on an Entity if the WebService's Id type is not Generic.
    /// </summary>
    /// <example>See ExtensionEntityWebService, where the Id type is long and not generic.</example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IdTypeAttribute : Attribute
    {
        public bool IsGenericForService { get; set; } = true;
        public bool IsGenericForWebService { get; set; } = true;
    }
}
