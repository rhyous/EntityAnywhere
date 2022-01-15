using Rhyous.Odata.Csdl;
using System;

namespace Rhyous.EntityAnywhere.Services
{
    public interface ICustomMetadataProvider
    {
        CsdlEntity Provide(Type entityType);
    }
}