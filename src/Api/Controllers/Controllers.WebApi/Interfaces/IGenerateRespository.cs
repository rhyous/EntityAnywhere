namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IGenerateRespository
    {
        RepositoryGenerationResult GenerateRepository();

        RepositorySeedResult InsertSeedData();
    }
}