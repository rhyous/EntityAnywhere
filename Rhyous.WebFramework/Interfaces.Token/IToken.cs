using System;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IToken : IId<long>, IAuditable
    {
        string Text { get; set; }
        long UserId { get; set; }
    }
}
