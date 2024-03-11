using System;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    public class EntityWithManyProperties
    {
        public int Id { get; set; }
        public int RelatedId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreateDate { get; set; }
    }
}