namespace Rhyous.EntityAnywhere.WebServices
{
    public interface ICustomWebServiceLoader
    {
        string PluginSubFolder { get; }
        void Load();
    }
}