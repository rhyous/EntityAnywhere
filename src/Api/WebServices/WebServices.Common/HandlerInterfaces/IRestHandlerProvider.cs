namespace Rhyous.EntityAnywhere.WebServices
{
    public interface IRestHandlerProvider
    {
        T Provide<T>();
    }
}