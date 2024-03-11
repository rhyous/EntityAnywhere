using System.Collections.Generic;
using System.Threading.Tasks;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface ISeedEntityHandler
    {
        Task<List<RepositorySeedResult>> Handle();
    }
}