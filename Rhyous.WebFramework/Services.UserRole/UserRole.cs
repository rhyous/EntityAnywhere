using Rhyous.WebFramework.Interfaces;
using System;

namespace Rhyous.WebFramework.Services
{
    public partial class UserRole : IUserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Active { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? LastUpdatedBy { get; set; }
    }
}
