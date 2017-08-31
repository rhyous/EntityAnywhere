namespace Rhyous.WebFramework.Interfaces
{
    using System.Collections.Specialized;
    using System.ComponentModel;

    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// While this method works with some other NameValueCollection instances, it was specifically designed to make getting a typed value back from an app.config or web.config using ConfigurationManager.AppSettings.
        /// Example usage to return a bool:
        ///   Web.config appseting
        ///     {appSettings file="dev.config"}
        ///         {add key="Setting1" value="true" /}
        ///     {/appSettings}
        ///   Line of code:  
        ///     ConfigurationManager.AppSettings.Get("Setting1", true);
        /// </summary>
        /// <typeparam name="T">The type to return.</typeparam>
        /// <param name="collection">The NameValueCollection, usually from ConfigurationManager.AppSettings.</param>
        /// <param name="key">The key is a string identifier of the item in the collection.</param>
        /// <param name="defaultValue">If the key is not found, or if the value is null, empty, or whitespace, or cannot be converted, this specified default value is used.</param>
        /// <returns></returns>
        public static T Get<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            var value = collection[key];
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (string.IsNullOrWhiteSpace(value) || !converter.IsValid(value))
            {
                return defaultValue;
            }
            return (T)(converter.ConvertFromInvariantString(value));
        }
    }
}
