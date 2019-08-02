using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhyous.WebFramework.WebServices
{
    [CustomWebService("$Entity$WebService", typeof($IEntity$WebService), typeof($Entity$), "$Entity$Service.svc")]
    public class $Entity$WebService : EntityWebService<$Entity$, $IEntity$, int, ServiceCommon<$Entity$, $IEntity$, int>>, $IEntity$WebService
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
