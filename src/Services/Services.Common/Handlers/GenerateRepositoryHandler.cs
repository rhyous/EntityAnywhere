using Rhyous.EntityAnywhere.Interfaces;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    class GenerateRepositoryHandler<TEntity, TInterface, TId> : IGenerateRepositoryHandler<TEntity, TInterface, TId>
           where TEntity : class, TInterface, new()
           where TInterface : IBaseEntity<TId>
           where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        private readonly IRepository<TEntity, TInterface, TId> _Repository;

        public GenerateRepositoryHandler(IRepository<TEntity, TInterface, TId> repository)
        {
            _Repository = repository;
        }
        
        public RepositoryGenerationResult GenerateRepository()
        {
            try { return _Repository.GenerateRepository() ?? throw new Exception("Unknown error. The Repository did not return a result"); }
            catch (Exception e)
            {
                return new RepositoryGenerationResult
                {
                    Name = typeof(TEntity).Name,
                    FailureReason = e.Message,
                    RepositoryReady = false
                };
            }
        }
    }
}