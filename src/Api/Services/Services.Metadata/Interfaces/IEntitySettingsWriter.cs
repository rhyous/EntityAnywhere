using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IEntitySettingsWriter
    {
        Task Write(MissingEntitySettings settings);
    }
}