namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IAuthorizationClientFactory
    {
        IAuthorizationClient Create(string token = null, string serviceUrl = null);
    }
}