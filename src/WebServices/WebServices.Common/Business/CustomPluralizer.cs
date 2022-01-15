using Rhyous.StringLibrary.Pluralization;

namespace Rhyous.EntityAnywhere.WebServices
{
    public class CustomPluralizer : ICustomPluralizer
    {
        public CustomPluralizer() { Configure(); }

        public void Configure()
        {
            foreach (var isoCode in new[] { "en", "en-US" })
            {
                if (IETFLanguageTagDictionary.Instance.TryGetValue(isoCode, out IPluralizer pluralizer))
                {
                    pluralizer.PluralizationDictionary?.Add("ProductInSuite", "ProductsInSuite");
                    pluralizer.PluralizationDictionary?.Add("ProductToUpgrade", "ProductsToUpgrade");
                }
            }
        }
    }
}
