namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IAuthenticationClientFactory
    {
        IAuthenticationClient Create(string serviceUrl = null);
    }
}