using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.AutomatedTests
{
    public class EntityTypeJsonProviderDictionary : Dictionary<string, Func<string, JObject, JToken>>
    {
        public EntityTypeJsonProviderDictionary()
        {
            Init();
        }

        private void Init()
        {
            Add("Edm.DateTime", (p, csdl) => { return DateTime.Now; });
            Add("Edm.DateTimeOffset", (p, csdl) => { return DateTimeOffset.Now; });
            Add("Edm.String", (p, csdl) => { return p + "01"; });
            Add("Edm.Int32", (p, csdl) => { return 127; });
            Add("Edm.Int64", (p, csdl) => { return 3000000027; });
            Add("Edm.Boolean", (p, csdl) => { return true; });
        }
    }
}
