using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IInsertSeedDataHandler<TEntity, TInterface, TId>
    {
        RepositorySeedResult InsertSeedData();
    }
}