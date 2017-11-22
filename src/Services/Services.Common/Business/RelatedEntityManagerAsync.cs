using Rhyous.Odata;
using Rhyous.Odata.Expand;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public class RelatedEntityManager<TEntity, TInterface, TId> : IGetRelatedEntitiesAsync<TEntity, TInterface, TId>
        where TEntity : class, TInterface, new()
        where TInterface : IId<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(TInterface entity, IEnumerable<ExpandPath> expandPaths = null)
        {
            return await GetRelatedEntitiesAsync(new[] { entity }, expandPaths);
        }        

        public async Task<List<RelatedEntityCollection>> GetRelatedEntitiesAsync(IEnumerable<TInterface> entities, IEnumerable<ExpandPath> expandPaths = null)
        {
            var list = new List<RelatedEntityCollection>();
            foreach (var accessor in RelatedEntityAccessors)
            {
                var relatedEntities = await accessor.GetRelatedEntitiesAsync(entities, expandPaths);
                if (relatedEntities != null && relatedEntities.Any())
                    list.AddRange(relatedEntities);
            }
            return list;
        }

        #region injectables
        /// <summary>
        /// Used for both caching and reusing existing clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal IEntityClientCache ClientsCache
        {
            get { return _ClientsCache ?? (_ClientsCache = new EntityClientCache()); }
            set { _ClientsCache = value; }
        } private IEntityClientCache _ClientsCache;

        /// <summary>
        /// Used for both caching and reusing existing mapping clients and is also used for dependency injection, for example, mocking in unit tests.
        /// </summary>
        internal Dictionary<string, IMappingEntityClientAsync> MappingClientsCache
        {
            get { return _MappingClientsCache ?? (_MappingClientsCache = new Dictionary<string, IMappingEntityClientAsync>()); }
            set { _MappingClientsCache = value; }
        } private Dictionary<string, IMappingEntityClientAsync> _MappingClientsCache;

        internal IRelatedEntitySorter<TInterface> Sorter
        {
            get { return _Sorter ?? (_Sorter = new RelatedEntitySorter<TInterface, TId>()); }
            set { _Sorter = value; }
        } private IRelatedEntitySorter<TInterface> _Sorter;
        
        internal AttributeEvaluator AttributeEvaluator
        {
            get { return _AttributeEvaluator ?? (_AttributeEvaluator = new AttributeEvaluator()); }
            set { _AttributeEvaluator = value; }
        } private AttributeEvaluator _AttributeEvaluator;

        internal List<IGetRelatedEntitiesAsync<TEntity, TInterface, TId>> RelatedEntityAccessors
        {
            get
            {
                return _RelatedEntityAccessors ??
                      (_RelatedEntityAccessors = new List<IGetRelatedEntitiesAsync<TEntity, TInterface, TId>>
                        {
                            new RelatedEntityExtensions<TEntity, TInterface, TId>() { ClientsCache = ClientsCache, Sorter = Sorter },
                            new RelatedEntityManyToOne<TEntity, TInterface, TId>() { ClientsCache = ClientsCache, Sorter = Sorter, AttributeEvaluator = AttributeEvaluator },
                            new RelatedEntityOneToMany<TEntity, TInterface, TId>() { ClientsCache = ClientsCache, Sorter = Sorter, AttributeEvaluator = AttributeEvaluator },
                            //new RelatedEntityManyToMany<TEntity, TInterface, TId>() { MappingClientsCache = MappingClientsCache, Sorter = Sorter, AttributeEvaluator = AttributeEvaluator },
                        });
            }
            set { _RelatedEntityAccessors = value; }
        }
        private List<IGetRelatedEntitiesAsync<TEntity, TInterface, TId>> _RelatedEntityAccessors;

        #endregion
    }
}