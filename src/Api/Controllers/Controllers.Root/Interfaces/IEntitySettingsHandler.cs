using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IEntitySettingsHandler
    {
        Task<EntitySettingsDictionaryDto> Handle();
    }
}