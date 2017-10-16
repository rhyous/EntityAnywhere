using Newtonsoft.Json;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class EntityClientAsync<TId> : EntityClientBase, IEntityClientAsync<TId>
    {
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

        public async Task<String> GetAddendaAsync(string id)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda");
        }

        public async Task<String> GetAddendaByEntityIdsAsync(List<string> ids)
        {
            return await HttpClientRunner.Run(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/Ids/Addenda", ids);
        }

        public async Task<String> GetAddendaByNameAsync(string id, string name)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/Addenda({name})");
        }

        public async Task<String> GetAllAsync()
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}");
        }

        public async Task<String> GetAsync(string idOrName)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({idOrName})");
        }

        public async Task<String> GetByCustomUrlAsync(string urlPart)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{urlPart}");
        }

        public async Task<String> GetByCustomUrlAsync(string urlPart, Func<string, HttpContent, Task<HttpResponseMessage>> httpMethod, HttpContent content)
        {
            return await HttpClientRunner.Run(httpMethod, $"{ServiceUrl}/{urlPart}", content);
        }

        public async Task<String> GetByIdsAsync(IEnumerable<TId> ids)
        {
            return await HttpClientRunner.Run(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}/Ids", ids);
        }

        public async Task<String> GetByQueryParametersAsync(string queryParameters)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}?{queryParameters}");
        }

        public async Task<String> GetMetadataAsync()
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}/$Metadata");
        }

        public async Task<string> GetPropertyAsync(string id, string property)
        {
            return await HttpClientRunner.Run(HttpClient.GetAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}");
        }

        public async Task<String> PatchAsync(string id, HttpContent content)
        {
            return await HttpClientRunner.Run(HttpClient.PatchAsync, $"{ServiceUrl}/{EntityPluralized}({id})", content, JsonSerializerSettings);
        }

        public async Task<String> PostAsync(HttpContent content)
        {
            return await HttpClientRunner.Run(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}", content, JsonSerializerSettings);
        }

        public async Task<String> PutAsync(string id, HttpContent content)
        {
            return await HttpClientRunner.Run(HttpClient.PutAsync, $"{ServiceUrl}/{EntityPluralized}({id})", content, JsonSerializerSettings);
        }

        public async Task<string> UpdatePropertyAsync(string id, string property, string value)
        {
            return await HttpClientRunner.Run(HttpClient.PostAsync, $"{ServiceUrl}/{EntityPluralized}({id})/{property}", value, JsonSerializerSettings);
        }
    }
}