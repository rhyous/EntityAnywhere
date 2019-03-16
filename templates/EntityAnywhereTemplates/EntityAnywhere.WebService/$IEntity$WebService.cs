using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace $safeprojectname$
{ 
    [ServiceContract]
    public interface I$IEntity$ : IEntityWebService<$IEntity$, int>, ICustomWebService
    {
    }
}
