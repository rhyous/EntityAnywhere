using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserToUserRole : IId
    {
        int UserId { get; set; }
        int UserTypeId { get; set; }
    }
}
