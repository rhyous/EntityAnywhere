
namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IEntityControllerListBuilder
    {
        void Build(IEnumerable<Type> entityTypes);
    }
}