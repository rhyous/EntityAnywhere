using Newtonsoft.Json;
using Rhyous.Wrappers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Collections;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class EnvironmentVariableAppSettings : IAppSettings, SimplePluginLoader.IAppSettings
    {
        public const string EnvironmentVariablePrefix = "EAF_";
        public EnvironmentVariableAppSettings()
        {

        }

        public NameValueCollection Collection
        {
            get { return _Collection ?? (_Collection = GetCollection()); }
        } private NameValueCollection _Collection;

        NameValueCollection SimplePluginLoader.IAppSettings.Settings => Collection;

        public NameValueCollection GetCollection()
        {
            var dict = Environment.GetEnvironmentVariables();
            var nvc = new NameValueCollection();
            foreach (DictionaryEntry item in dict)
            {
                if (item.Key.ToString().StartsWith(EnvironmentVariablePrefix))
                {
                    var key = item.Key.ToString()
                                  .Substring(EnvironmentVariablePrefix.Length);
                    nvc.Add(key, item.Value.ToString());
                }
            }
            return nvc;
        }
    }
}
