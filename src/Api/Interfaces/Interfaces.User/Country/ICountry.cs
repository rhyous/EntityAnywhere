namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>An interface for a country.</summary>
    public interface ICountry : IBaseEntity<int>, IName
    {
        int GeoId { get; set; }
        string TwoLetterIsoCode { get; set; }
        string ThreeLetterIsoCode { get; set; }
    }
}
