using Newtonsoft.Json;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;

namespace Rhyous.WebFramework.Clients
{
    public class EntityClientBase : IEntityClientBase
    {
        public const string EntitySuffix = "EntityUrl";
        public const string ServiceSuffix = "Service.svc";
        public const string EntityHost = "EntityHost";

        public EntityClientBase()
        {
        }

        public EntityClientBase(HttpClient httpClient, bool useMicrosoftDateFormat = false) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
            _HttpClient = httpClient;
        }

        public EntityClientBase(bool useMicrosoftDateFormat) 
            : this(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
        }

        public EntityClientBase(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
        }

        /// <summary>
        /// A JsonSerializerSettings object, that allows you to customize serialization settings.
        /// </summary>
        public virtual JsonSerializerSettings JsonSerializerSettings { get; set; }

        /// <inheritdoc />
        public virtual IHttpContextProvider HttpContextProvider
        {
            get { return _HttpContextProvider ?? (_HttpContextProvider = new HttpContextProvider()); }
            set { _HttpContextProvider = value; }
        } internal protected IHttpContextProvider _HttpContextProvider;

        /// <inheritdoc />
        public virtual HttpClient HttpClient
        {
            get { return _HttpClient ?? (_HttpClient = new HttpClient()); }
            set { _HttpClient = value; }
        } internal protected HttpClient _HttpClient;

        protected internal virtual NameValueCollection AppSettings
        {
            get { return _AppSettings ?? (_AppSettings = ConfigurationManager.AppSettings); }
            set { _AppSettings = value; }
        } internal protected NameValueCollection _AppSettings;

        /// <inheritdoc />
        public virtual string ServiceUrl
        {
            get { return _ServiceUrl ?? (_ServiceUrl = AppSettings.Get($"{Entity}{EntitySuffix}", $"{AppSettings.Get(EntityHost, HttpContextProvider.WebHost)}/{Entity}{ServiceSuffix}")); }
            set { _ServiceUrl = value; }
        } internal protected string _ServiceUrl;

        /// <inheritdoc />
        public virtual string Entity { get; protected set; }

        /// <inheritdoc />
        public virtual string EntityPluralized => PluralizationDictionary.Instance.GetValueOrDefault(Entity);
    }
}
