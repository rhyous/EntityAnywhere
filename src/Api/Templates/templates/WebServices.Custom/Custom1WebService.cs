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
    /// The custom webservice for Custom1.
    /// </summary>
    [CustomWebService("Custom1WebService", typeof(ICustom1WebService), null, "Custom1Service.svc")]
    public sealed class Custom1WebService : EntityWebService<Custom1, ICustom1, long>,
                                            ICustom1WebService
    {
        private readonly IRestHandlerProvider _RestHandlerProvider;

        public Custom1WebService(IRestHandlerProvider restHandlerProvider) 
            : base(restHandlerProvider)
        {
            _RestHandlerProvider = relatedEntityProvider;
        }

        [ExcludeFromCodeCoverage] // Exclude because it simply forwards on.
        public async bool NewEndpoint()
        {
            // Create a handler, register it, and call it here.
            throw new NotImplementedException();
        }
    }
}