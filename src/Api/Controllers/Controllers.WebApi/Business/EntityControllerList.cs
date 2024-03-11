namespace Rhyous.EntityAnywhere.WebApi
{
    public class EntityControllerList : IEntityControllerList
    {
        public List<Type> ControllerTypes { get; } = new List<Type>();
    }
}
