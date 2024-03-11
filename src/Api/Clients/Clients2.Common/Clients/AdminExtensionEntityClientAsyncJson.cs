namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminExtensionEntityClientAsync : ExtensionEntityClientAsync, 
                                                   IAdminExtensionEntityClientAsync
    {
        public AdminExtensionEntityClientAsync(IEntityClientConnectionSettings entityClientSettings,
                                               IAdminHttpClientRunner httpClientRunner)
            : base(entityClientSettings, httpClientRunner)
        {
        }
    }
}