using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IEntitySettingsProvider
    {
        Task<IDictionary<string, EntitySetting>> GetAsync();
    }
}