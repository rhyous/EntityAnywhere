using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    internal class InsertSeedDataHandler<TEntity, TInterface, TId> : IInsertSeedDataHandler<TEntity, TInterface, TId>
        where TEntity : class, TInterface
        where TInterface : IId<TId>
    {
        private readonly IServiceCommon<TEntity, TInterface, TId> _Service;

        public InsertSeedDataHandler(IServiceCommon<TEntity, TInterface, TId> service)
        {
            _Service = service;
        }

        public virtual RepositorySeedResult InsertSeedData() => _Service.InsertSeedData();
    }
}