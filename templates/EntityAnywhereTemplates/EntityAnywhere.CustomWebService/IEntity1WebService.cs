using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace $safeprojectname$
{ 
[ServiceContract]
public interface IEntity1 : IEntityWebService<Entity1, int>
{
}
}
