using Rhyous.EntityAnywhere.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IEntitySettingsHandler
    {
        Task<Dictionary<string, EntitySetting>> Handle();
    }
}