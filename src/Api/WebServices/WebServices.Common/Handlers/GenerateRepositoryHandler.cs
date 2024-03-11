using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    internal class GenerateRepositoryHandler<TEntity, TInterface, TId> : IGenerateRepositoryHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface
        where TInterface : IId<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;

        public GenerateRepositoryHandler(IServiceCommon<TEntity, TInterface, TId> service)
        {
            _Service = service;
        }
        public virtual RepositoryGenerationResult GenerateRepository() => _Service.GenerateRepository();
    }
}