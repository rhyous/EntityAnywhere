using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.RelatedEntities
{
    public class RelatedEntityProvider<TEntity, TInterface, TId> : IRelatedEntityProvider<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {

        internal string[] ParametersToHandle = new[] { "$expand" };
        private readonly ExpandParser _ExpandParser;

        public RelatedEntityProvider(IRelatedEntityManager<TEntity, TInterface, TId> relatedEntityManager,
                                     IRelatedEntityCollater<TEntity, TId> relatedEntityCollater,
                                     ExpandParser expandParser)
        {
            RelatedEntityManager = relatedEntityManager;
            Collater = relatedEntityCollater;
            _ExpandParser = expandParser;
        }

        public async Task ProvideAsync(IEnumerable<OdataObject<TEntity, TId>> entities, NameValueCollection urlParameters)
        {
            if (entities == null || !entities.Any() || entities.All(e => e == null))
                return;
            var expandParameters = GetHandleableParameters(urlParameters, ParametersToHandle);
            var relatedEntityCollection = await RelatedEntityManager.GetRelatedEntitiesAsync(entities.Select(o => o.Object), _ExpandParser.Parse(expandParameters));
            if (relatedEntityCollection != null && relatedEntityCollection.Any())
                Collater.Collate(entities, relatedEntityCollection);
        }

        internal NameValueCollection GetHandleableParameters(NameValueCollection urlParameters, IEnumerable<string> parametersToHandle)
        {
            var collection = new NameValueCollection();
            foreach (var param in parametersToHandle)
            {
                var paramValue = urlParameters.Get(param, string.Empty);
                if (!string.IsNullOrWhiteSpace(paramValue))
                    collection.Add(param, paramValue);
            }
            return collection;
        }

        internal virtual IGetRelatedEntitiesAsync<TEntity, TInterface, TId> RelatedEntityManager { get; set; }

        internal IRelatedEntityCollater<TEntity, TId> Collater { get; set; }
    }
}