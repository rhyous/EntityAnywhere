namespace Rhyous.WebFramework.Interfaces
{
    public interface ICountry : IEntity<int>, IName
    {
        string TwoLetterIsoCode { get; set; }
        string ThreeLetterIsoCode { get; set; }
    }
}
