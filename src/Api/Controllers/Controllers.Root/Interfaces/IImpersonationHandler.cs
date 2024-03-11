using Rhyous.EntityAnywhere.Interfaces;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IImpersonationHandler
    {
        Task<Token> HandleAsync(int roleId);
    }
}