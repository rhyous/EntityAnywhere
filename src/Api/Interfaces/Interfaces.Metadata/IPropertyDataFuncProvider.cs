using Rhyous.Odata.Csdl;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IPropertyDataFuncProvider
    {
        ICustomPropertyDataFuncs Provide();
    }
}