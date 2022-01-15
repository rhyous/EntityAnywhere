using Rhyous.Odata.Csdl;

namespace Rhyous.EntityAnywhere.Services
{
    public interface IPropertyFuncProvider
    {
        IFuncList<string> Provide();
    }
}