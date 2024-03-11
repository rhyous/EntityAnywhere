using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Rhyous.EntityAnywhere.Entities
{
    public class CountrySeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects { get; } = GetCountries();

        internal static List<object> GetCountries()
        {
            var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.Name));
            return regions.Where(r=>!string.IsNullOrWhiteSpace(r.ThreeLetterISORegionName))
                          .Select(r => new Country
            {
                Id = r.GeoId,
                Name = r.EnglishName,
                ThreeLetterIsoCode = r.ThreeLetterISORegionName,
                TwoLetterIsoCode = r.TwoLetterISORegionName,
                CurrencySymbol = r.CurrencySymbol
            })
            .Distinct(new CountryComparer())
            .Select(c => c as object)
            .ToList();
        }
    }
}
