using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface ICustomMetadataProvider
    {
        CsdlEntity Provide(Type entityType);
    }
}