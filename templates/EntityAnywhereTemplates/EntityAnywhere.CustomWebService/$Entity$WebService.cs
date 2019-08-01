using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhyous.WebFramework.WebServices
{
    [CustomWebService("$Entity$WebService", typeof($IEntity$WebService), null)]
    public class $Entity$WebService : $IEntity$WebService
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
