namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IHttpClientFactory
    {
        IHttpClient GetHttpClient(string baseAddress = null);
    }
}