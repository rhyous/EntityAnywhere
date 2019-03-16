using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace $safeprojectname$
{
    [CustomWebService("Entity1WebService", typeof(I$IEntity$WebService), typeof($IEntity$))]
    public class $IEntity$WebService : EntityWebService<$IEntity$, I$IEntity$, int, ServiceCommon<$IEntity$, I$IEntity$, int>>, I$IEntity$Service
    {
    }
}
