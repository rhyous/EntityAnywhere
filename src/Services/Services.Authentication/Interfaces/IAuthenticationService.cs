using Rhyous.EntityAnywhere.Interfaces;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IAuthenticationService : IDisposable
    {
        Task<IToken> AuthenticateAsync(Credentials creds, string IpAddress);
    }
}