﻿using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Attributes;

namespace Rhyous.EntityAnywhere.Clients2.Common.Tests
{
    [DisplayNameProperty("Name")]
    [AlternateKey("Name")]
    public class EntityString : IEntityString
    {
        public string Id { get; set; }
        [IgnoreTrim]
        public string Name { get; set; }
    }
}