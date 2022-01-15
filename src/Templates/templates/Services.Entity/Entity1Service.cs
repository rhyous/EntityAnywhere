using Rhyous.Odata;
using Rhyous.StringLibrary;
using Rhyous.WebFramework.Clients;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.Services
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Entity1Service : ServiceCommon<Entity1, IEntity1, int>
    {
        public Entity1Service(ILogger logger) : base(logger)
        {
        }
    }
}