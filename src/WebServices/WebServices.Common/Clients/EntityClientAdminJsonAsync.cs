using Newtonsoft.Json;
using Rhyous.WebFramework.Interfaces;
using System.Configuration;
using System.Net.Http;

namespace Rhyous.WebFramework.Clients
{
    public class EntityClientAdminAsync : EntityClientAsync
    {
        #region Constructors
        public EntityClientAdminAsync() => Init();

        public EntityClientAdminAsync(string entity) : this() => Entity = entity;

        public EntityClientAdminAsync(HttpClient httpClient, bool useMicrosoftDateFormat = false)
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
            _HttpClient = httpClient;
        }

        public EntityClientAdminAsync(bool useMicrosoftDateFormat)
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
        }

        public EntityClientAdminAsync(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
            Init();
        }
        #endregion

        protected internal override void Init()
        {
            if (_IsInitialized)
                return;
            _IsInitialized = true;
            var entityAdminToken = ConfigurationManager.AppSettings.Get($"{Entity}{HeaderAdminToken}", "");
            if (string.IsNullOrWhiteSpace(entityAdminToken))
                entityAdminToken = ConfigurationManager.AppSettings.Get(HeaderAdminToken, "");
            HttpClient.DefaultRequestHeaders.Add(HeaderAdminToken, entityAdminToken);
        } private bool _IsInitialized;
    }
}
