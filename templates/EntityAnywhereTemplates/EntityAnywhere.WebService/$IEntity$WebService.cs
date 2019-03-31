using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.WebServices;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Rhyous.WebFramework.WebServices
{ 
    [ServiceContract]
    public interface $IEntity$WebService : IEntityWebService<$Entity$, int>, ICustomWebService
    {
    }
}
