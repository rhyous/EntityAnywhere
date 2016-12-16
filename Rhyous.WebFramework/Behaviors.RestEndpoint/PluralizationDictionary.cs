using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public class PlaralizationDictionary : Dictionary<string, string>, IDictionaryDefaultValueProvider<string, string>
    {
        #region Singleton

        private static readonly Lazy<RestDictionary> Lazy = new Lazy<RestDictionary>(() => new RestDictionary());

        public static RestDictionary Instance { get { return Lazy.Value; } }

        internal PlaralizationDictionary()
        {
            Init();
        }

        #endregion

        public void Init()
        {
            // Todo: Get from repository or something
            Add("Addendum", "Addenda");
        }

        public string DefaultValueProvider(string key)
        {
            return key + "s";
        }
    }
}