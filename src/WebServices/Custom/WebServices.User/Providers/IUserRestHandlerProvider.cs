using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.WebServices
{
    public interface IUserRestHandlerProvider : IRestHandlerProviderAlternateKey<User, IUser, long, string>
    {
        IFilterHandler FilterHandler { get; }
    }
}