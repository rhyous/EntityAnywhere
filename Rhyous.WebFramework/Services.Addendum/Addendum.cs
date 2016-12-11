using System;
using Rhyous.WebFramework.Interfaces;

namespace Rhyous.WebFramework.Services
{
    public partial class Addendum : IAddendum
    {
        public long Id { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? LastUpdatedBy { get; set; }
    }
}
