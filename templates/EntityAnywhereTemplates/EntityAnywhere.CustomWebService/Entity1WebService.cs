using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$
{
    [CustomWebService("Entity1WebService", typeof(IEntity1WebService), typeof(IEntity1))]
    public class Entity1WebService : EntityWebService<Entity1, IEntity1, int, ServiceCommon<Entity1, IEntity1, int>>, IEntity1Service
    {
    }
}
