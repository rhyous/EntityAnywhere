using System.Collections.Specialized;

namespace Rhyous.WebFramework.Interfaces
{
    public interface IHeaderValidator
    {
        bool IsValid(NameValueCollection headers);
        long UserId { get; set; }
    }
}