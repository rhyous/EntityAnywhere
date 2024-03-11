namespace Rhyous.EntityAnywhere.Services
{
    public interface IServiceHandlerProvider
    {
        T Provide<T>();
    }
}