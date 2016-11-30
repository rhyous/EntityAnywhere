using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserToUserType : IId
    {
        int UserId { get; set; }
        int UserTypeId { get; set; }
    }
}
