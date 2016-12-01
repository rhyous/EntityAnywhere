using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IToken : IId, IAuditable
    {
        string Text { get; set; }
        int UserId { get; set; }
    }
}
