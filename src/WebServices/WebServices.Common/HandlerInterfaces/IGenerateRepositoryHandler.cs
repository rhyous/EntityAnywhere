using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGenerateRepositoryHandler<TEntity, TInterface, TId>
    {
        RepositoryGenerationResult GenerateRepository();
    }
}