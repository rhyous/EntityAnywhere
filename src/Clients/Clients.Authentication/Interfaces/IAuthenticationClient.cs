using Rhyous.EntityAnywhere.Interfaces;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Clients2
{
    public interface IAuthenticationClient
    {
        Task<IToken> AuthenticateAsync(ICredentials credentials);
        Task<IToken> AuthenticateAsync(string user, string password);
    }
}