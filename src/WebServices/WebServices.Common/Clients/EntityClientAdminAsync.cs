using Newtonsoft.Json;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Configuration;
using System.Net.Http;

namespace Rhyous.WebFramework.Clients
{
    public class EntityClientAdminAsync<TEntity, TId> : EntityClientAsync<TEntity, TId>
        where TEntity : class, new()
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {        
        #region Constructors
        public EntityClientAdminAsync()
        {
            Init();
        }

        public EntityClientAdminAsync(HttpClient httpClient, bool useMicrosoftDateFormat = false) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
            _HttpClient = httpClient;
        }

        public EntityClientAdminAsync(bool useMicrosoftDateFormat) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
        }

        public EntityClientAdminAsync(JsonSerializerSettings jsonSerializerSettings) : base(jsonSerializerSettings)
        {
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