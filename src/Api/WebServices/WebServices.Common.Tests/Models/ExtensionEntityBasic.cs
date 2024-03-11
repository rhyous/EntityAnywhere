using System;
using Rhyous.EntityAnywhere.Interfaces;

namespace Rhyous.EntityAnywhere.WebServices.Common.Tests
{
    public class ExtensionEntityBasic : IExtensionEntity
    {
        public long Id { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? LastUpdated { get; set; }
        public long CreatedBy { get; set; }
        public long? LastUpdatedBy { get; set; }
    }
}
