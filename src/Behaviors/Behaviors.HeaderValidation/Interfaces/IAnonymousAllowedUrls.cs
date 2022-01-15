using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface IAnonymousAllowedUrls
    {
        HashSet<string> Urls { get; }
    }
}