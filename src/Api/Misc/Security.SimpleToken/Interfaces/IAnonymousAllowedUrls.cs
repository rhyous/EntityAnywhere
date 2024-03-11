using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Security
{
    public interface IAnonymousAllowedUrls
    {
        HashSet<string> Urls { get; }
    }
}