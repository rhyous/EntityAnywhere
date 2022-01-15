using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IHttpClientRunnerNoHeaders
    {
        Task<string> Run(HttpMethod method, string url, bool forwardExceptions = true);
        Task<string> Run(HttpMethod method, string url, HttpContent content, bool forwardExceptions = true);
        Task<string> Run<T1>(HttpMethod method, string url, T1 content, JsonSerializerSettings settings = null, bool forwardExceptions = true);
        Task<TResult> RunAndDeserialize<TContent, TResult>(HttpMethod method, string url, TContent content, JsonSerializerSettings settings = null, bool forwardExceptions = true);
        Task<TResult> RunAndDeserialize<TResult>(HttpMethod method, string url, bool forwardExceptions = true);
        Task<TResult> RunAndDeserialize<TResult>(HttpMethod method, string url, HttpContent content, bool forwardExceptions = true);
        Task<Stream> RunAndReturnStream(HttpMethod method, string url, bool forwardExceptions = true);
        Task<Stream> RunAndReturnStream(HttpMethod method, string url, HttpContent content, bool forwardExceptions = true);
    }
}