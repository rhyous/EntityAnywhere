using Newtonsoft.Json;
using Rhyous.Wrappers;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// This combines web.config appsettings with values from a Json file, if that file exists.
    /// </summary>
    public class AppSettingsContainer : IAppSettings
    {
        internal const string ApplicationSettingsPath = "ApplicationSettingsPath";
        private readonly IFileIO _File;

        public AppSettingsContainer(NameValueCollection appSettings, IFileIO file)
        {
            Collection = appSettings;
            _File = file;
            GetSecureJsonAppSettings();
        }

        public NameValueCollection Collection { get; }

        internal void GetSecureJsonAppSettings()
        {
            var jsonFileLocation = Collection?.Get(ApplicationSettingsPath);
            if (!string.IsNullOrEmpty(jsonFileLocation) && _File.Exists(jsonFileLocation))
            {
                var jsonSettingsText = _File.ReadAllText(jsonFileLocation);
                var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSettingsText);
                foreach (var setting in settings)
                {
                    if (string.IsNullOrWhiteSpace(Collection[setting.Key]))
                    {
                        Collection[setting.Key] = setting.Value;
                    }
                }
            }
        }
    }
}