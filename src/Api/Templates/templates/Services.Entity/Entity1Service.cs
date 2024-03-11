using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.EntityAnywhere.Entities;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Rhyous.EntityAnywhere.Services
{
    /// <summary>
    /// The custom service for Entity1.
    /// </summary>
    public sealed class Entity1Service : ServiceCommon<Entity1, IEntity1, int>, IEntity1Service
    {
        public Entity1Service(IServiceHandlerProvider serviceHandlerProvider) 
            : base(serviceHandlerProvider)
        {
        }
    }
}