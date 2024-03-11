using Rhyous.Collections;
using Rhyous.Odata;
using Rhyous.EntityAnywhere.Attributes;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services;
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    /// <summary>
    /// The custom webservice for Entity1.
    /// </summary>
    [CustomWebService("Entity1WebService", typeof(IEntity1WebService), typeof(Entity1), "Entity1Service.svc")]
    public sealed class Entity1WebService : EntityWebService<Entity1, IEntity1, int>,
                                            IEntity1WebService
    {
        public Entity1WebService(IRestHandlerProvider restHandlerProvider) 
            : base(restHandlerProvider)
        {
        }

        [ExcludeFromCodeCoverage] // Exclude because it simply forwards on.
        public async Task<OdataObjectCollection<Entity1, int>> NewEndpoint()
        {
            // Create a handler, register it, and call it here.
            throw new NotImplementedException();
        }
    }
}