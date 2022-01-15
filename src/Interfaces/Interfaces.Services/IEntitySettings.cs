namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntitySettings
    {
        string ServiceUrl { get; }
        string Token { get; }
        bool IsAdmin { get; }
    }
}