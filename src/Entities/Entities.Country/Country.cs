using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Interfaces;
using Rhyous.Odata.Csdl;

namespace Rhyous.WebFramework.Entities
{
    /// <summary>
    /// Gets countries from the C# Region info
    /// </summary>
    [ReadOnlyEntity]
    [Entity(CanGenerateRepository = false)]
    public class Country : BaseEntity<int>, ICountry
    {
        public string Name { get; set; }
        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }
    }
}