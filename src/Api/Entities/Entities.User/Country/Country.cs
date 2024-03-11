using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.Odata.Csdl;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.EntityAnywhere.Entities
{
    /// <summary>
    /// Gets countries from the C# Region info
    /// </summary>
    [CountrySeedData]
    [EntitySettings(Description = "A list of countries.",
                    Group = "Users, Roles, and Authorization",
                    GroupDescription = "A group for Users, Roles, and Authorization entities.")]
    public class Country : AuditableEntity<int>, ICountry
    {
        public int GeoId { get; set; }
        public string Name { get; set; }
        //[StringLength(2, MinimumLength = 2)]
        public string TwoLetterIsoCode { get; set; }
        //[StringLength(3, MinimumLength = 3)]
        public string ThreeLetterIsoCode { get; set; }
        //[StringLength(10, MinimumLength = 1)]
        public string CurrencySymbol { get; set; }
    }
}
