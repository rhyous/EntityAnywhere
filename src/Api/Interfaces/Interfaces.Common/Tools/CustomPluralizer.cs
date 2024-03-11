using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.StringLibrary.Pluralization;

namespace Rhyous.EntityAnywhere.Tools
{
    public class CustomPluralizer : ICustomPluralizer
    {
        private readonly IETFLanguageTagDictionary _IETFLanguageTagDictionary;

        public CustomPluralizer(IETFLanguageTagDictionary IETFLanguageTagDictionary)
        {
            _IETFLanguageTagDictionary = IETFLanguageTagDictionary;
            Configure();
        }

        public void AddCustomPluralization(string isoCode, string singular, string plural)
        {
            if (_IETFLanguageTagDictionary.TryGetValue(isoCode, out IPluralizer pluralizer))
            {
                pluralizer.PluralizationDictionary?.Add("Culdesac", "Culsdesac");
            }
        }

        public void Configure()
        {
            foreach (var isoCode in new[] { "en", "en-US" })
            {
                // Example:
                // AddCustomPluralization(isoCode, "culdesac", "culsdesac")
            }
        }
    }
}
