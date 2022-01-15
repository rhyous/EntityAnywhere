using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Behaviors
{
    public interface IUrlFilter
    {
        HashSet<string> RequestUrls { get; }
        HashSet<string> ResponseUrls { get; }
    }
}