using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IUser : IUserBase, IAuditable
    {
        string Password { get; set; }
        string Salt { get; set; }
        bool Active { get; set; }
        bool ExternalAuth { get; set; }
    }
}
