using Rhyous.Collections;
using Rhyous.EntityAnywhere.Exceptions;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// This class allows for checking for duplicates entities.
    /// A duplicate is an entity with the same value for the Alternate Key property.
    /// </summary>
    /// <typeparam name="TEntity">The Entity type.</typeparam>
    /// <typeparam name="TInterface">The Entity Inteface type.</typeparam>
    /// <typeparam name="TId">The type of the Entity's Id field.</typeparam>
    /// <typeparam name="TAltKey">The type of the Entity's Alternate Key property.</typeparam>
    class DuplicateEntityPreventer<TEntity, TInterface, TId, TAltKey> : IDuplicateEntityPreventer<TEntity, TInterface, TId, TAltKey>
        where TEntity : class, TInterface, new()
        where TInterface : IBaseEntity<TId>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TAltKey : IComparable, IComparable<TAltKey>, IEquatable<TAltKey>
    {
        private readonly IEntityInfoAltKey<TEntity, TAltKey> _EntityInfoAltKey;
        private readonly IGetByPropertyValuesHandler<TEntity, TInterface, TId> _GetByPropertyValuesHandler;
        private readonly AlternateKeyTracker<TEntity, TAltKey> _AlternateKeyTracker;

        public DuplicateEntityPreventer(IEntityInfoAltKey<TEntity, TAltKey> entityInfoAltKey,
                                        IGetByPropertyValuesHandler<TEntity, TInterface, TId> getByPropertValuesHandler,
                                        AlternateKeyTracker<TEntity, TAltKey> alternateKeyTracker)
        {
            _EntityInfoAltKey = entityInfoAltKey;
            _GetByPropertyValuesHandler = getByPropertValuesHandler;
            _AlternateKeyTracker = alternateKeyTracker;
        }

        /// <summary>
        /// This method checks for duplicates entities.
        /// A duplicate is an entity with the same value for the Alternate Key property.
        /// This checks for duplicates in 3 ways:
        /// 1. Duplicate in the posted data
        /// 2. Duplicate in that another post is currently posting a duplicate
        /// 3. Duplicates already in the repository
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        public async Task CheckAsync(IEnumerable<TInterface> entities)
        {
            var method = _EntityInfoAltKey.PropertyExpressionMethod;
            var altKeyGroups = entities.GroupBy(e => method(e as TEntity)).ToList();
            var duplicates = new HashSet<TInterface>();
            var dupAltKeys = new HashSet<TAltKey>();
            foreach (var group in altKeyGroups)
            {
                // 1. Duplicates in the posted data
                if (group.Count() > 1)
                {
                    dupAltKeys.Add(group.Key);
                    duplicates.AddRange(group);
                }

                // 2. Duplicates in simultaneous posts
                if (!_AlternateKeyTracker.Add(group.Key))
                {
                    dupAltKeys.Add(group.Key);
                    duplicates.AddRange(group);
                }
            }
            // 3. Duplicates already in the repository
            var altKeys = entities.Select(e => method(e as TEntity))
                                  .Where(altKey => !dupAltKeys.Contains(altKey));
            if (altKeys.Any())
            {
                var dupsInRepo = (await _GetByPropertyValuesHandler.GetAsync(_EntityInfoAltKey.AlternateKeyProperty, altKeys))?
                                                                   .ToList();
                if (dupsInRepo != null && dupsInRepo.Any())
                    duplicates.AddRange(dupsInRepo);
            }

            if (duplicates != null && duplicates.Any())
                throw new DuplicateKeyException(_EntityInfoAltKey.AlternateKeyProperty, $"Duplicate {typeof(TEntity).Name}(s) detected: {(string.Join(", ", duplicates))}");
        }

        /// <summary>
        /// Allows for cleaning up the AlternateKeyTracker ConcurrentHashSet after the entities are 
        /// post to the repoistory.
        /// </summary>
        /// <param name="entities">The entities, the expectation is that these entities were successfully added to the repo.</param>
        public void RemoveTracked(IEnumerable<TInterface> entities)
        {
            var method = _EntityInfoAltKey.PropertyExpressionMethod;
            var altKeys = entities.Select(e => method(e as TEntity));
            foreach (var altKey in altKeys)
            {
                _AlternateKeyTracker.TryRemove(altKey);
            }
        }
    }
}