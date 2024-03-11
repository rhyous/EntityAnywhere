
namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IEntityActionNameAttributeUpdater
    {
        void Update(Type controllerType, Type entityType);
    }
}