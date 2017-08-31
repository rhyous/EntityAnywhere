using Newtonsoft.Json;
using Rhyous.WebFramework.Behaviors;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Clients
{
    public class MappingEntityClient<T, Tid, E1Tid, E2Tid> : EntityClient<T,Tid>, IMappingEntityClientAsync<T, Tid, E1Tid, E2Tid>
        where T : class, IMappingEntity<E1Tid, E2Tid>, new()
        where Tid : IComparable, IComparable<Tid>, IEquatable<Tid>
        where E1Tid : IComparable, IComparable<E1Tid>, IEquatable<E1Tid>
        where E2Tid : IComparable, IComparable<E2Tid>, IEquatable<E2Tid>
    {

        #region Constructors
        public MappingEntityClient() { }

        public MappingEntityClient(bool useMicrosoftDateFormat) : base(useMicrosoftDateFormat) { }

        public MappingEntityClient(JsonSerializerSettings jsonSerializerSettings) : base(jsonSerializerSettings) { }
        #endregion

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
            return TaskRunner.RunSynchonously(GetByMappedEntityAsync, Entity1Pluralized, ids);
        }
        
        public async Task<List<OdataObject<T>>> GetByE1IdsAsync(IEnumerable<E1Tid> ids)
        {
            return await GetByMappedEntityAsync(Entity1Pluralized, ids.ToList());
        }

        public List<OdataObject<T>> GetByE2Ids(IEnumerable<E2Tid> ids)
        {
            return GetByE2Ids(ids.ToList());
        }

        public List<OdataObject<T>> GetByE2Ids(List<E2Tid> ids)
        {
            return TaskRunner.RunSynchonously(GetByMappedEntityAsync, Entity2Pluralized, ids);
        }

        public async Task<List<OdataObject<T>>> GetByE2IdsAsync(IEnumerable<E2Tid> ids)
        {
            return await GetByMappedEntityAsync(Entity2Pluralized, ids.ToList());
        }

        private async Task<List<OdataObject<T>>> GetByMappedEntityAsync<Eid>(string pluralizedEntityName, List<Eid> ids)
        {
            return await HttpClientRunner.RunAndDeserialize<List<Eid>, List<OdataObject<T>>>(HttpClient.PostAsync, $"{ServiceUrl}/Api/{EntityPluralized}/{pluralizedEntityName}/Ids", ids);
        }
    }
}