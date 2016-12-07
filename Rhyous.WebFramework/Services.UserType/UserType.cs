using Rhyous.WebFramework.Interfaces;
using System;

namespace Rhyous.WebFramework.Services
{
    public partial class UserType : IUserType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? LastUpdatedBy { get; set; }        
    }
}
