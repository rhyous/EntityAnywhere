using Newtonsoft.Json;
using Rhyous.Odata.Csdl;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Clients2;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public class CsdlDocumentProvider
    {
        public CsdlDocumentProvider(IEntityClientConfig config)
        {
            Config = config;
        }

        private string BaseUri => string.IsNullOrWhiteSpace(Config.EntitySubpath)
                                   ? Config.EntityWebHost
                                   : StringConcat.WithSeparator('/', Config.EntityWebHost, Config.EntitySubpath);

        public IEntityClientConfig Config { get; }

        public CsdlDocument Provide()
        {
            var metadataUrl = StringConcat.WithSeparator('/', BaseUri, "Service/$Metadata");
            var json = new HttpClient().GetAsync(metadataUrl).Result;
            var entityListResultJson = json.Content.ReadAsStringAsync().Result;

            var doc = JsonConvert.DeserializeObject<CsdlDocument>(entityListResultJson);

            var schemaKvp = doc.Schemas.First();
            var schemaJson = schemaKvp.Value.ToString();
            var schema = JsonConvert.DeserializeObject<CsdlSchema>(schemaJson);
            doc.Schemas[schemaKvp.Key] = schema;
            var updates = new List<KeyValuePair<string, object>>();
            foreach (var kvp in schema.Entities)
            {
                var entityName = kvp.Key;
                var entity = JsonConvert.DeserializeObject<CsdlEntity>(kvp.Value.ToString());
                updates.Add(new KeyValuePair<string, object>(entityName, entity));
            }
            foreach (var kvp in updates)
            {
                schema.Entities[kvp.Key] = kvp.Value;
            }
            return doc;
        }
    }
}
