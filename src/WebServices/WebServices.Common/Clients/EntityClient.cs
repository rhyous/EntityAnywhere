using Newtonsoft.Json;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class EntityClient<T, Tid> : IEntityClient<T, Tid>
        where T : class, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
    {
        public HttpClient HttpClient
        {
            get { return _HttpClient ?? (_HttpClient = new HttpClient()); }
            set { _HttpClient = value; }
        } private HttpClient _HttpClient;

        public IHttpContextProvider HttpContextProvider
        {
            get { return _HttpContextProvider ?? (_HttpContextProvider = new HttpContextProvider()); }
            set { _HttpContextProvider = value; }
        } private IHttpContextProvider _HttpContextProvider;

        public string ServiceUrl
        {
            get { return _ServiceUrl ?? (_ServiceUrl = ConfigurationManager.AppSettings.Get($"{Entity}WebServiceUrl", $"{HttpContextProvider.WebHost}/{typeof(T).Name}Service.svc")); }
            set { _ServiceUrl = value; }
        } internal string _ServiceUrl;

        public string Entity => typeof(T).Name;
        public string EntityPluralized => PluralizationDictionary.Instance.GetValueOrDefault(Entity);

        public bool Delete(string id)
        {
            Task<HttpResponseMessage> response = HttpClient.DeleteAsync($"{ServiceUrl}/{EntityPluralized}({id})");
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
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/{EntityPluralized}({idOrName})");
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
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/{EntityPluralized}");
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

        /// <summary>
        /// Include only the part of the url after the https://hsotname/path/EntityService.svc/
        /// This is useful for services with custom endpoints, so a client can call the custom
        /// endpoint without having to create an EntityClient child object.
        /// </summary>
        /// <param name="urlPart"></param>
        /// <returns></returns>
        public List<OdataObject<T>> GetByCustomUrl(string urlPart)
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/{urlPart}");
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

        public List<OdataObject<T>> GetAll(string queryParameters)
        {
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/{EntityPluralized}");
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

        public List<OdataObject<T>> GetByIds(IEnumerable<Tid> ids) {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PostAsync($"{ServiceUrl}/{EntityPluralized}/Ids", postContent);
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
            return GetByIds((IEnumerable<Tid>)ids);
        }

        public string GetProperty(string id, string property)
        {
            Task<HttpResponseMessage> response = HttpClient.DeleteAsync($"{ServiceUrl}/{EntityPluralized}({id})/{property}");
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
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/{EntityPluralized}/$Metadata");
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
            Task<HttpResponseMessage> response = HttpClient.PatchAsync($"{ServiceUrl}/{EntityPluralized}({id})", postContent);
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
            Task<HttpResponseMessage> response = HttpClient.PatchAsync($"{ServiceUrl}/{EntityPluralized}", postContent);
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
            Task<HttpResponseMessage> response = HttpClient.PutAsync($"{ServiceUrl}/{EntityPluralized}({id})", postContent);
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
            Task<HttpResponseMessage> response = HttpClient.PostAsync($"{ServiceUrl}/{EntityPluralized}({id})/{property}", postContent);
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
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/{EntityPluralized}({id})/Addenda");
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
            Task<HttpResponseMessage> response = HttpClient.GetAsync($"{ServiceUrl}/{EntityPluralized}({id})/Addenda({name})");
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

        public List<Addendum> GetAddendaByEntityIds(List<string> ids)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PostAsync($"{ServiceUrl}/{EntityPluralized}/Ids/Addenda", postContent);
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
    }
}