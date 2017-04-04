using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    public class PluralizationDictionary : Dictionary<string, string>, IDictionaryDefaultValueProvider<string, string>
    {
        #region Singleton

        private static readonly Lazy<PluralizationDictionary> Lazy = new Lazy<PluralizationDictionary>(() => new PluralizationDictionary());

        public static PluralizationDictionary Instance { get { return Lazy.Value; } }

        internal PluralizationDictionary()
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