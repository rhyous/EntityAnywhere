using Rhyous.WebFramework.Interfaces;
using System;

namespace Rhyous.WebFramework.Services
{
    public partial class Token : IToken
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long UserId { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? LastUpdatedBy { get; set; }
    }
}
