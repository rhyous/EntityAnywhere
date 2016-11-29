using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IUserType : IId, IAuditable
    {
        string Type { get; set; }
    }
}
