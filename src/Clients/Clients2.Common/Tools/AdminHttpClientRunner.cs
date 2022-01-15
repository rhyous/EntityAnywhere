using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminHttpClientRunner : HttpClientRunner, IAdminHttpClientRunner
    {
        public AdminHttpClientRunner(IHttpClientFactory httpClientFactory, IAdminHeaders headers)
            : base(httpClientFactory, headers)
        {
        }
    }
}