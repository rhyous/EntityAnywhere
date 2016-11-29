using Rhyous.Db.Auditable;
using Rhyous.WebFramework.Interfaces;
using System;

namespace Rhyous.WebFramework.Repositories
{
    public class User : IUser, IAuditableTable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string OrganizationId { get; set; }
        public bool Active { get; set; }
        public bool ExternalAuth { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? LastUpdatedBy { get; set; }        
    }
}
