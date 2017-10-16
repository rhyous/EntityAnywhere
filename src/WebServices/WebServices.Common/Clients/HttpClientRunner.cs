using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    /// <summary>
    /// This class is used by EntityClient to run common HttpClient commands asynchronously. Whether calling GET, POST, PUT, PATCH, DELETE, etc, the way to make the call and get the response back is standardized.
    /// </summary>
    public class HttpClientRunner
    {
        /// <summary>
        /// Runs any asynchonous method that takes in a url string and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.GetAsync();
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public static async Task<string> Run(Func<string, Task<HttpResponseMessage>> method, string url)
        {
            HttpResponseMessage response = await method(url);
            return await response.Content.ReadAsStringAsync();
        }
        
        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post as an HttpContent.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public static async Task<string> Run(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, HttpContent content)
        {
            HttpResponseMessage response = await method(url, content);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content that is not yet in HttpContent form and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post.</param>
        /// <returns>A string, where the string is the actually result of the HttpClient method.</returns>
        public static async Task<string> Run<T1>(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, T1 content, JsonSerializerSettings settings = null)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(content, settings), Encoding.UTF8, "application/json");
            return await Run(method, url, postContent);
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.GetAsync();
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public static async Task<Stream> RunAndReturnStream(Func<string, Task<HttpResponseMessage>> method, string url)
        {
            HttpResponseMessage response = await method(url);
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content and returns HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post as an HttpContent.</param>
        /// <returns>A Task{stream}, where string is the actually result of the HttpClient method.</returns>
        public static async Task<Stream> RunAndReturnStream(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, HttpContent content)
        {
            HttpResponseMessage response = await method(url, content);
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content that is not yet in HttpContent form and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post.</param>
        /// <returns>A stream, where the stream is the actually result of the HttpClient method.</returns>
        public static async Task<Stream> RunAndReturnStream<T1>(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, T1 content, JsonSerializerSettings settings = null)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(content, settings), Encoding.UTF8, "application/json");
            return await RunAndReturnStream(method, url, postContent);
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.GetAsync();
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <returns>A Task{TResult}, where the instance of TResult is instantiated by deserializing the string result of the HttpClient method us JsonConvert.DeserializeObject.</returns>
        public static async Task<TResult> RunAndDeserialize<TResult>(Func<string, Task<HttpResponseMessage>> method, string url)
        {
            var jsonResult = await Run(method, url);
            try { return JsonConvert.DeserializeObject<TResult>(jsonResult); } catch { return default(TResult); };
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post as an HttpContent.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public static async Task<TResult> RunAndDeserialize<TResult>(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, HttpContent content)
        {
            var jsonResult = await Run(method, url, content);
            return JsonConvert.DeserializeObject<TResult>(jsonResult);
        }

        /// <summary>
        /// Runs any asynchonous method that takes in a url string and POST content that is not yet in HttpContent form and returns a deserialized HttpResponseMessage. This was built for HttpClient methods such as HttpClient.PostAsync(content);
        /// </summary>
        /// <typeparam name="TResult">The type to return.</typeparam>
        /// <param name="method">Any asynchonous method that returns HttpResponseMessage</param>
        /// <param name="url">The url for HttpClient to call.</param>
        /// <param name="content">The content to post.</param>
        /// <returns>A Task{string}, where string is the actually result of the HttpClient method.</returns>
        public static async Task<TResult> RunAndDeserialize<T1, TResult>(Func<string, HttpContent, Task<HttpResponseMessage>> method, string url, T1 content, JsonSerializerSettings settings = null)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(content, settings), Encoding.UTF8, "application/json");
            return await RunAndDeserialize<TResult>(method, url, postContent);
        }
    }
}
