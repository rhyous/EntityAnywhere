using System;

namespace Rhyous.WebFramework.Attributes
{
    public class ExplicitServiceContractAttribute : IExplicitServiceContract
    {
        public ExplicitServiceContractAttribute(Type serviceContract) { ServiceContract = serviceContract; }
        public Type ServiceContract { get; set; }
    }
}
