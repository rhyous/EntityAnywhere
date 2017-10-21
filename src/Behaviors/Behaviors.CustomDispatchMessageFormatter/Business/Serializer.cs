using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Rhyous.WebFramework.Behaviors
{
    public class Serializer : ISerializer
    {
        public byte[] Json(object obj)
        {
            var serializer = new JsonSerializer() { ContractResolver = ContractResolver.Instance };
            using (var stream = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream, Encoding.UTF8))
                {
                    using (var writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, obj);
                        sw.Flush();
                        return stream.ToArray();
                    }
                }
            }
        }
    }
}