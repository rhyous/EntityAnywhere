namespace Rhyous.WebFramework.Interfaces
{
    public interface ICountry : IBaseEntity<int>, IName
    {
        string TwoLetterIsoCode { get; set; }
        string ThreeLetterIsoCode { get; set; }
    }
}
