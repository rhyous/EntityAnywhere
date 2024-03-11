using Rhyous.EntityAnywhere.Interfaces;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IPasswordManager
    {
        int DefaultPasswordLength { get; }
        void SetOrHashPassword(IUser user, bool passwordChanged);
        void SetOrHashPassword(IEnumerable<IUser> users);
    }
}
