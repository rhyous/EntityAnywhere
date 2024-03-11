namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface ICustomPluralizer
    {
        void Configure();
        void AddCustomPluralization(string isoCode, string singular, string plural);
    }
}