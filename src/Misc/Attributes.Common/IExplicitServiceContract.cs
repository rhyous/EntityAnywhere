using System;

namespace Rhyous.WebFramework.Attributes
{
    public interface IExplicitServiceContract
    {
        Type ServiceContract { get; set; }
    }
}
