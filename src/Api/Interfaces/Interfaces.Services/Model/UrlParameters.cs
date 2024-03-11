using System.Collections.Specialized;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public class UrlParameters : IUrlParameters
    {
        public NameValueCollection Collection { get; set; }
    }
}
