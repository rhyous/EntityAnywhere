using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;

namespace Rhyous.EntityAnywhere.Repositories.Common.Tests
{
    [EntitySettings(Group = "Audit")]
    public class Log : BaseEntity<int>, ILog
    {
        public string Message { get; set; }
    }
}
