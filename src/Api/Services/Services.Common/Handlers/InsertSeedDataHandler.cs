using Rhyous.Collections;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Collections.Generic;
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
        private readonly IUpdateHandler<TEntity, TInterface, TId> _UpdateHandler;
        private readonly Type _EntityType;
        private readonly IUrlParameters _UrlParameters;

        public InsertSeedDataHandler(IRepository<TEntity, TInterface, TId> repository, 
            IUpdateHandler<TEntity,TInterface,TId> updateHandler,
            IUrlParameters urlParameters)
        {
            _Repository = repository;
            _UpdateHandler = updateHandler;
            _EntityType = typeof(TEntity);
            _UrlParameters = urlParameters;
        }
        
        public RepositorySeedResult InsertSeedData()
        {
            try
            {                
                var seedAttribute = _EntityType.GetCustomAttributes()?
                                                  .FirstOrDefault(a => (typeof(EntitySeedDataAttribute)
                                                  .IsAssignableFrom(a.GetType()))) as EntitySeedDataAttribute;

                var result = new RepositorySeedResult
                {
                    Name = _EntityType.Name,
                    EntityHasSeedData = seedAttribute != null
                };

                if (!result.EntityHasSeedData)
                    return result;
       
                result.SeedSuccessful = TryProcessSeedData(seedAttribute);

                if (!result.SeedSuccessful)
                {
                    result.FailureReason = "Some seed entities were not created or updated.";
                    return result;
                }

                return result;
            }
            catch (Exception e)
            {
                return new RepositorySeedResult
                {
                    Name = _EntityType.Name,
                    FailureReason = e.Message,
                    SeedSuccessful = false
                };
            }
        }

        private bool TryProcessSeedData(EntitySeedDataAttribute seedAttribute)
        {           
            var entitySeedList = seedAttribute.Objects.Cast<TEntity>().ToList();
            var entitySeedDataIds = entitySeedList.Select(e => e.Id);
            var existingEntityRecords = _Repository.Get(entitySeedDataIds)?.Cast<TEntity>().ToList();

            var isInsertSuccessful = TryInsertNewSeedData(entitySeedList, existingEntityRecords, out List <TEntity> insertedSeedData);

            var allowOverwrite = _UrlParameters.Collection.Get("Overwrite", false);
            var unprocessedSeedData = entitySeedList.Where(e => !insertedSeedData.Any(x => x.Id.Equals(e.Id))).ToList();
            var isUpdateSuccessful = !allowOverwrite || TryOverwriteExistingSeedData(unprocessedSeedData, existingEntityRecords);           

            return isInsertSuccessful && isUpdateSuccessful;
        }

        private bool TryInsertNewSeedData(List<TEntity> entitySeedList, List<TEntity> existingEntityRecords, out List<TEntity> seedDataToInsert)
        {
            var existingEntityRecordIds = existingEntityRecords?.Select(x => x.Id);

            seedDataToInsert = !existingEntityRecordIds.NullOrEmpty() ?
                          entitySeedList.Where(e => !existingEntityRecordIds.Contains(e.Id)).ToList() : entitySeedList;

            if (seedDataToInsert.NullOrEmpty())
                return true;

            var insertedEntities = _Repository.InsertSeedData(seedDataToInsert);
            return seedDataToInsert.Count() == insertedEntities.Count();
        }

        private bool TryOverwriteExistingSeedData(List<TEntity> entitySeedListToUpdate, List<TEntity> existingEntityRecords)
        {
            if (entitySeedListToUpdate.NullOrEmpty())
                return true;

            var editableProperties = _EntityType.GetEditableProperties();

            if (editableProperties.NullOrEmpty())
                return true;

            var entitiesToUpdate = from entityInSeedList in entitySeedListToUpdate
                                   let matchingEntity = existingEntityRecords.First(y => y.Id.Equals(entityInSeedList.Id))
                                   where editableProperties.Any(prop => !prop.GetValue(entityInSeedList).Equals(prop.GetValue(matchingEntity)))
                                   select entityInSeedList;

            if (entitiesToUpdate.NullOrEmpty())
                return true;

            var updatedEntities = _UpdateHandler.Update(entitiesToUpdate, editableProperties.Select(x => x.Name).ToArray());
            return updatedEntities.Count() == entitiesToUpdate.Count();            
        }
    }
}