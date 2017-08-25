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
using Rhyous.WebFramework.Interfaces;
using System.Linq;

namespace Rhyous.WebFramework.Clients
{
    public class MappingEntityClient<T, Tid, E1Tid, E2Tid> : EntityClient<T,Tid>, IMappingEntityClient<T, Tid, E1Tid, E2Tid>
        where T : class, IMappingEntity<E1Tid, E2Tid>, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {      

        public string Entity1 { get { return _Entity1 ?? (_Entity1 =  typeof(T).GetMappedEntity1()); } }
        private string _Entity1;

        public string Entity1Pluralized { get { return _Entity1Pluralized ?? (_Entity1Pluralized = typeof(T).GetMappedEntity1Pluralized()); } }
        private string _Entity1Pluralized;

        public string Entity1Property { get { return _Entity1Property ?? (_Entity1Property = typeof(T).GetMappedEntity1Property()); } }
        private string _Entity1Property;
        
        public string Entity2 { get { return _Entity2 ?? (_Entity2 = typeof(T).GetMappedEntity2()); } }
        private string _Entity2;

        public string Entity2Pluralized { get { return _Entity2Pluralized ?? (_Entity2Pluralized = typeof(T).GetMappedEntity2Pluralized()); } }
        private string _Entity2Pluralized;

        public string Entity2Property { get { return _Entity2Property ?? (_Entity2Property = typeof(T).GetMappedEntity2Property()); } }
        private string _Entity2Property;
        
        public List<OdataObject<T>> GetByE1Ids(IEnumerable<E1Tid> ids)
        {
            return GetByE1Ids(ids.ToList());
        }

        public List<OdataObject<T>> GetByE1Ids(List<E1Tid> ids)
        {
            return GetByMappedEntity(Entity1Pluralized, ids);
        }
        
        public List<OdataObject<T>> GetByE2Ids(IEnumerable<E2Tid> ids)
        {
            return GetByE2Ids(ids.ToList());
        }

        public List<OdataObject<T>> GetByE2Ids(List<E2Tid> ids)
        {
            return GetByMappedEntity(Entity2Pluralized, ids);
        }

        private List<OdataObject<T>> GetByMappedEntity<Eid>(string pluralizedEntityName, List<Eid> ids)
        {
            HttpContent postContent = new StringContent(JsonConvert.SerializeObject(ids), Encoding.UTF8, "application/json");
            Task<HttpResponseMessage> response = HttpClient.PostAsync($"{ServiceUrl}/Api/{EntityPluralized}/{pluralizedEntityName}/Ids", postContent);
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
    }
}