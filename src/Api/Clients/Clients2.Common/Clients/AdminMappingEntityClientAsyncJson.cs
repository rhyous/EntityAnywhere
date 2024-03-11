namespace Rhyous.EntityAnywhere.Clients2
{
    public class AdminMappingEntityClientAsync 
        : MappingEntityClientAsync, IAdminMappingEntityClientAsync
    {
        public AdminMappingEntityClientAsync(IEntityClientConnectionSettings entityClientSettings,
                                             IMappingEntitySettings mappingEntitySettings,
                                             IAdminHttpClientRunner httpClientRunner)
       : base(entityClientSettings, mappingEntitySettings, httpClientRunner)
        {
        }
    }
}