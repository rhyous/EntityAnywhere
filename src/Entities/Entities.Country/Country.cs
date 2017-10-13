using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// Gets countries from the C# Region info
    /// </summary>
    [ReadOnlyEntity]
    public class Country : Entity<int>, ICountry
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
    }
}
