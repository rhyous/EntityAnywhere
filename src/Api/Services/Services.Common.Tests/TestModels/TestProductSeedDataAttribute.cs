using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.EntityAnywhere.Services.Common.Tests.TestModels
{
    class TestProductSeedDataAttribute : EntitySeedDataAttribute
    {
        public override List<object> Objects => new List<Product>()
        {
            new Product { Id = 1, Name = "Prod1" , Type = 1, Version = "V1"},
            new Product { Id = 2, Name = "Prod2" , Type = 2, Version = "V2"},
            new Product { Id = 3, Name = "Prod3", Type = 3, Version = "V3" }
        }.ToList<object>();
    }
}
