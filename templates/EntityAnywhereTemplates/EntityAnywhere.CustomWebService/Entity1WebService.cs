using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
$endif$using System.Text;

namespace $safeprojectname$
{

[CustomWebService("Entity1WebService", typeof(IEntity1WebService), typeof(File))]
public class Entity1WebService : ServicesCommon
{
}
}
