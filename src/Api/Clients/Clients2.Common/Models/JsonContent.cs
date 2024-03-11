using System.Net.Http;
using System.Text;

namespace Rhyous.EntityAnywhere.Clients2.Common
{
    public class JsonContent : StringContent
    {
        private const string JsonMediaType = "application/json";

        public JsonContent(string json, Encoding encoding = null, string mediaType = JsonMediaType) : base(json, encoding ?? Encoding.UTF8, JsonMediaType)
        {
        }
    }
}