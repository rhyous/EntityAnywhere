using System;
using System.Collections.Generic;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IEntityWebServiceBuilder
    {
        void Build(IEnumerable<Type> entityTypes);
    }
}