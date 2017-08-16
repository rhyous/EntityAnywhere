using Newtonsoft.Json;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Services;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Rhyous.WebFramework.Entities;

namespace Rhyous.WebFramework.Clients
{
    public class ClientCommon<T, Tid> : IClientCommon<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        public HttpClient HttpClient
        {
            get { return _HttpClient ?? (_HttpClient = new HttpClient()); }
            set { _HttpClient = value; }
        } private HttpClient _HttpClient;

        public string ServiceUrl
        {
            get => _ServiceUrl ?? (_ServiceUrl = ConfigurationManager.AppSettings.Get($"{Entity}WebServiceUrl", ""));
            set => throw new NotImplementedException();
        } internal string _ServiceUrl;

        public string Entity => typeof(T).Name;
        public string EntityPluralized => PluralizationDictionary.Instance.GetValueOrDefault(Entity);

        public bool Delete(string id)
        {
            Task<HttpResponseMessage> response = HttpClient.DeleteAsync($"{ServiceUrl}/Api/{EntityPluralized}({id})");
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return Convert.ToBoolean(result);
            }
            catch { return false; }            
        }

        public OdataObject<T> Get(string idOrName)
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/Api/{EntityPluralized}({idOrName})");
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<OdataObject<T>>(result);
            }
            catch { return null; }
        }

        public List<OdataObject<T>> GetAll()
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/Api/{EntityPluralized}");
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<List<OdataObject<T>>>(result);
            }
            catch { return null; }
        }

        public List<OdataObject<T>> GetByIds(List<Tid> ids)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PostAsync($"{ServiceUrl}/Api/{EntityPluralized}/Ids", postContent);
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<List<OdataObject<T>>>(result);
            }
            catch { return null; }
        }

        public string GetProperty(string id, string property)
        {
            Task<HttpResponseMessage> response = HttpClient.DeleteAsync($"{ServiceUrl}/Api/{EntityPluralized}({id})/{property}");
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return result;
            }
            catch { return null; }
        }

        public EntityMetadata<T> GetMetadata()
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/Api/{EntityPluralized}/$Metadata");
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<EntityMetadata<T>>(result);
            }
            catch { return null; }
        }
        public OdataObject<T> Patch(string id, PatchedEntity<T> patchedEntity)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(patchedEntity), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PatchAsync($"{ServiceUrl}/Api/{EntityPluralized}({id})", postContent);
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<OdataObject<T>>(result);
            }
            catch { return null; }
        }
        
        public List<OdataObject<T>> Post(List<T> entity)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PatchAsync($"{ServiceUrl}/Api/{EntityPluralized}", postContent);
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<List<OdataObject<T>>>(result);
            }
            catch { return null; }
        }

        public OdataObject<T> Put(string id, T entity)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PutAsync($"{ServiceUrl}/Api/{EntityPluralized}({id})", postContent);
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<OdataObject<T>>(result);
            }
            catch { return null; }
        }

        public string UpdateProperty(string id, string property, string value)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PostAsync($"{ServiceUrl}/Api/{EntityPluralized}({id})/{property}", postContent);
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                return readAsStringTask.Result;
            }
            catch { return null; }
        }

        public List<Addendum> GetAddenda(string id)
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/Api/{EntityPluralized}({id})/Addenda");
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<List<Addendum>>(result);
            }
            catch { return null; }
        }

        public Addendum GetAddendaByName(string id, string name)
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/Api/{EntityPluralized}({id})/Addenda({name})");
            try
            {
                response.Wait();
                var readAsStringTask = response.Result.Content.ReadAsStringAsync();
                readAsStringTask.Wait();
                var result = readAsStringTask.Result;
                return JsonConvert.DeserializeObject<Addendum>(result);
            }
            catch { return null; }
        }
    }
}