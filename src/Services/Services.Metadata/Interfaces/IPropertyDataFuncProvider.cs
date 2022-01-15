using Rhyous.Odata.Csdl;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IPropertyDataFuncProvider
    {
        IFuncList<string, string> Provide();
    }
}