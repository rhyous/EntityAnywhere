using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;

namespace Entities.SafeShare
{
    public class School : AuditableEntity<long>, ISchool
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }

    public interface ISchool : IBaseEntity<long>, IAuditable, IName
    {
        string Country { get; set; }
        string State { get; set; }
        string City { get; set; }

    }
}