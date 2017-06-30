using System;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Entities
{
    [AlternateId("Username")]
    public partial class User : IUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsHashed { get; set; }
        public bool Enabled { get; set; }
        public bool ExternalAuth { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int CreatedBy { get; set; }
        public int? LastUpdatedBy { get; set; }
    }
}
