namespace Rhyous.EntityAnywhere.Clients2
{
    public interface INamedFactory<T>
    {
        T Create(string name);
    }
}