using System.Collections.Generic;
using System;
using Rhyous.WebFramework.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rhyous.WebFramework.Services
{
    [Table("Users")]
    public partial class User : UserBase, IUser
    {
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }
        public bool ExternalAuth { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int CreatedBy { get; set; }
        public int? LastUpdatedBy { get; set; }
        
        public ICollection<string> Types { get; set; }
    }
}
