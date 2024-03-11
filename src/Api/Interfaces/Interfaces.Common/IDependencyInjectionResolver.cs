namespace Rhyous.EntityAnywhere.Interfaces
{
    /// <summary>
    /// An interface defining the dependency injection resolver. The DI framework should be abstracted. 
    /// We Should inject this instead of the real DI framework.
    /// </summary>
    public interface IDependencyInjectionResolver
    {
        T Resolve<T>();
    }
}