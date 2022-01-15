using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminEntityClientAsync : EntityClientAsync, IAdminEntityClientAsync
    {
        public AdminEntityClientAsync(IEntityClientConnectionSettings entityClientSettings,
                                      IAdminHttpClientRunner httpClientRunner) 
            : base(entityClientSettings, httpClientRunner)
        {
        }
    }
}