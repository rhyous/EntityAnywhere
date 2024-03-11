namespace Rhyous.EntityAnywhere.WebApi
{
    public interface IEntityControllerBuilder
    {
        Type Build(Type entityType);
    }
}