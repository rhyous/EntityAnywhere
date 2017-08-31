using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    internal class HttpClientRunner
    {
        public static async Task<string> Run(Func<string, Task<HttpResponseMessage>> method, string url)
        {
            HttpResponseMessage response = await method(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> Run(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, HttpContent content)
        {
            HttpResponseMessage response = await method(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<TResult> RunAndDeserialize<TResult>(Func<string, Task<HttpResponseMessage>> method, string url)
        {
            var jsonResult = await Run(method, url);
            return JsonConvert.DeserializeObject<TResult>(jsonResult);
        }

        public static async Task<TResult> RunAndDeserialize<TResult>(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, HttpContent content)
        {
            var jsonResult = await Run(method, url, content);
            return JsonConvert.DeserializeObject<TResult>(jsonResult);
        }

        public static async Task<TResult> RunAndDeserialize<T1, TResult>(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, T1 content, JsonSerializerSettings settings = null)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(content, settings), Encoding.UTF8, "application/json");
            return await RunAndDeserialize<TResult>(method, url, postContent);
        }
    }
}
