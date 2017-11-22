using Newtonsoft.Json;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class EntityClientAsync : EntityClientBase, IEntityClientAsync
    {
        public EntityClientAsync() { }

        public EntityClientAsync(string entity) { Entity = entity; }
        
        public EntityClientAsync(string entity, HttpClient httpClient, bool useMicrosoftDateFormat = false) 
            : base(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
            Entity = entity;
        }

        public EntityClientAsync(string entity, bool useMicrosoftDateFormat) 
            : base(useMicrosoftDateFormat ? new JsonSerializerSettings { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat } : null)
        {
            Entity = entity;
        }

        public EntityClientAsync(string entity, JsonSerializerSettings jsonSerializerSettings)
            : base (jsonSerializerSettings)
        {
            Entity = entity;
        }
        
        public async Task<bool> DeleteAsync(string id)
        {
            return await HttpClientRunner.RunAndDeserialize<bool>(HttpClient.DeleteAsync, $"{ServiceUrl}/{EntityPluralized}({id})");
        }

        public async Task<string> GetAddendaAsync(string id)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda");
        }

        public async Task<string> GetAddendaByEntityIdsAsync(List<string> ids)
        {
            return await HttpClientRunner.Run(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/Ids/Addenda", ids);
        }

        public async Task<string> GetAddendaByNameAsync(string id, string name)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda({name})");
        }

        public async Task<string> GetAllAsync(string urlParameters = null)
        {
            var url = $"{ServiceUrl}/{EntityPluralized}";
            url = AppendUrlParameters(urlParameters, url);
            return await HttpClientRunner.Run(HttpClient.GetAsync, url);
        }

        public async Task<string> GetAsync(string idOrName, string urlParameters = null)
        {
            var url = $"{ServiceUrl}/{EntityPluralized}({idOrName})";
            url = AppendUrlParameters(urlParameters, url);
            return await HttpClientRunner.Run(HttpClient.GetAsync, url);
        }

        public async Task<string> GetByCustomUrlAsync(string urlPart)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{urlPart}");
        }

        public async Task<string> GetByCustomUrlAsync(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, HttpContent content)
        {
            return await HttpClientRunner.Run(httpMethod, $"{ServiceUrl}/{urlPart}", content);
        }

        public async Task<string> GetByCustomUrlAsync(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, object content)
        {
            return await HttpClientRunner.Run(httpMethod, $"{ServiceUrl}/{urlPart}", content);
        }

        public async Task<string> GetByIdsAsync(IEnumerable<string> ids, string urlParameters = null)
        {
            var url = $"{ServiceUrl}/{EntityPluralized}/Ids";
            url = AppendUrlParameters(urlParameters, url);
            return await HttpClientRunner.Run(HttpClient.PostAsync, url, ids);
        }

        /// <inheritdoc />
        public async Task<string> GetByPropertyValuesAsync(string property, List<string> values, string urlParameters = null)
        {
            var url = $"{ServiceUrl}/{EntityPluralized}/{property}/Values";
            url = AppendUrlParameters(urlParameters, url);
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(values, JsonSerializerSettings), Encoding.UTF8, "application/json");
            return await HttpClientRunner.Run(HttpClient.PostAsync, url, postContent);
        }

        public async Task<string> GetByQueryParametersAsync(string queryParameters)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}?{queryParameters}");
        }

        public async Task<string> GetMetadataAsync()
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}/$Metadata");
        }

        public async Task<string> GetPropertyAsync(string id, string property)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}");
        }

        public async Task<string> PatchAsync(string id, HttpContent content)
        {
            return await HttpClientRunner.Run(HttpClient.PatchAsync, $"{ServiceUrl}/{EntityPluralized}({id})", content, JsonSerializerSettings);
        }

        public async Task<string> PostAsync(HttpContent content)
        {
            return await HttpClientRunner.Run(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}", content, JsonSerializerSettings);
        }

        public async Task<string> PutAsync(string id, HttpContent content)
        {
            return await HttpClientRunner.Run(HttpClient.PutAsync, $"{ServiceUrl}/{EntityPluralized}({id})", content, JsonSerializerSettings);
        }

        public async Task<string> UpdatePropertyAsync(string id, string property, string value)
        {
            return await HttpClientRunner.Run(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}", value, JsonSerializerSettings);
        }
        
        protected static string AppendUrlParameters(string urlParameters, string url)
        {
            if (!string.IsNullOrWhiteSpace(urlParameters))
                url += urlParameters.StartsWith("?") ? urlParameters : $"?{urlParameters}";
            return url;
        }
    }
}