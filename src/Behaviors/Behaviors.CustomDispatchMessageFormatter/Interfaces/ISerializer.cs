namespace Rhyous.WebFramework.Behaviors
{
    public interface ISerializer
    {
        byte[] Json(object obj);
    }
}