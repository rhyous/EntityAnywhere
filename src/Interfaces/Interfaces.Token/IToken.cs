using System;

namespace Rhyous.WebFramework.Interfaces
{
    public partial interface IToken : IEntity<long>, IAuditable
    {
        string Text { get; set; }
        long UserId { get; set; }
    }
}
