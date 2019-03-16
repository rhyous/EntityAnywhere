using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$
{
    [CustomWebService("Entity1WebService", typeof($IEntity$WebService), typeof($Entity$))]
    public class $Entity$WebService : EntityWebService<$Entity$, $IEntity$, int, ServiceCommon<$IEntity$, $IEntity$, int>>, $IEntity$Service
    {
    }
}
