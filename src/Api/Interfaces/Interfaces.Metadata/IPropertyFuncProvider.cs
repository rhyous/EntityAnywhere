using Rhyous.Odata.Csdl;

namespace Rhyous.EntityAnywhere.Interfaces
{
    public interface IPropertyFuncProvider
    {
        ICustomPropertyFuncs Provide();
    }
}