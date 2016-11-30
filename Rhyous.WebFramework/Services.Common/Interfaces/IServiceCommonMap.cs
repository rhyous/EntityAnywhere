using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IServiceCommonMap<Tinterface>
    {
        string PrimaryEntity { get; }
        string SecondaryEntity { get; }
        List<Tinterface> GetByPropertyId(int id, string propertyName);
    }
}