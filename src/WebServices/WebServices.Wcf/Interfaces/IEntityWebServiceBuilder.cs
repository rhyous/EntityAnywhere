using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IEntityWebServiceBuilder
    {
        void Build(IEnumerable<Type> entityTypes);
    }
}