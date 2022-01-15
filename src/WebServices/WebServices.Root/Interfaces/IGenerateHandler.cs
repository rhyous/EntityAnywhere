using System.Collections.Generic;
using System.Threading.Tasks;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IGenerateHandler
    {
        Task<List<RepositoryGenerationResult>> Handle();
    }
}