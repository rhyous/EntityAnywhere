using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserRole : IId, IName, IDescription, IAuditable, IActivateable
    {
    }
}
