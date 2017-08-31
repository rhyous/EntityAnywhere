﻿using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Behaviors
{
    /// <summary>
    /// This class is tool to pluralize entities. It is a singleton. 
    /// It is currently naive and could use significant enhancements.
    /// It implements IDictionaryDefaultValueProvider{string, string} so that it can use the GetValueOrDefault method.
    /// </summary>
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
            if (key.EndsWith("y", StringComparison.OrdinalIgnoreCase))
            {
                return key.Substring(0, key.Length - 1) + "ies";
            }
            foreach (var esChars in EsCharacters)
            {
                if (key.EndsWith(esChars, StringComparison.OrdinalIgnoreCase))
                    return key + "es";
            }
            return key + "s";
        }

        public string DefaultValue => null;

        public List<string> EsCharacters { get; } = new List<string> { "ch", "s", "sh", "x", "z" };
    }
}