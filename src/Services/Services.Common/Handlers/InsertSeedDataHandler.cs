using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Linq;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Services
{
    class InsertSeedDataHandler<TEntity, TInterface, TId> : IInsertSeedDataHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;

        public InsertSeedDataHandler(IRepository<TEntity, TInterface, TId> repository)
        {
            _Repository = repository;
        }
        
        public RepositorySeedResult InsertSeedData()
        {
            try
            {
                var seedAttribute = typeof(TEntity).GetCustomAttributes()?
                                                  .FirstOrDefault(a => (typeof(EntitySeedDataAttribute)
                                                  .IsAssignableFrom(a.GetType()))) as EntitySeedDataAttribute;
                var result = new RepositorySeedResult
                {
                    Name = typeof(TEntity).Name,
                    EntityHasSeedData = seedAttribute != null
                };
                if (!result.EntityHasSeedData)
                    return result;
                var entitySeedList = seedAttribute.Objects.Cast<TEntity>().ToList();
                var ids = entitySeedList.Select(e => e.Id);
                var existingEntities = _Repository.Get(ids);
                if (existingEntities == null || !existingEntities.Any())
                {
                    var addedEntities = _Repository.Create(entitySeedList)?.ToList();
                    result.SeedSuccessful = addedEntities.Count == entitySeedList.Count;
                    if (!result.SeedSuccessful)
                    {
                        result.FailureReason = "Some seed entities where not created.";
                        return result;
                    }
                    return result;
                }
                else
                {
                    var entitiesNotAlreadyAdded = entitySeedList.Where(e => existingEntities.Cast<TEntity>().None(ee => ee.Id.Equals(e.Id))).ToList();
                    result.SeedSuccessful = entitiesNotAlreadyAdded == null || !entitiesNotAlreadyAdded.Any();
                    if (result.SeedSuccessful)
                        return result;
                    var addedEntities = _Repository.Create(entitiesNotAlreadyAdded)?.ToList();
                    result.SeedSuccessful = addedEntities.Count == entitiesNotAlreadyAdded.Count;
                    if (!result.SeedSuccessful)
                    {
                        result.FailureReason = "Some seed entities where not created.";
                        return result;
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                return new RepositorySeedResult
                {
                    Name = typeof(TEntity).Name,
                    FailureReason = e.Message,
                    SeedSuccessful = false
                };
            }
        }
    }
}