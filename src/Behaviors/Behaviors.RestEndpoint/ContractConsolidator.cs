using Rhyous.EntityAnywhere.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors
{
    internal class ContractConsolidator
    {
        internal static void ConsolidateToSingleContract(IExplicitServiceContract attribute, IDictionary<string, ContractDescription> implementedContracts)
        {
            //var attribute = ;
            if (attribute != null && attribute.ServiceContract != null)
            {
                ConsolidateByAttribute(attribute, implementedContracts);
                return;
            }
            ConsolidateByInheritance(implementedContracts);
        }

        internal static void ConsolidateByInheritance(IDictionary<string, ContractDescription> implementedContracts)
        {
            var contracts = implementedContracts.ToList();
            foreach (var contract in contracts)
            {
                foreach (var otherContract in contracts.Where(c => c.Key != contract.Key))
                {
                    if (contract.Value.ContractType.IsAssignableFrom(otherContract.Value.ContractType))
                    {
                        implementedContracts.Remove(contract.Key);
                        if (implementedContracts.Count == 1)
                            break;
                    }
                }
            }
        }

        internal static void ConsolidateByAttribute(IExplicitServiceContract attribute, IDictionary<string, ContractDescription> implementedContracts)
        {
            var keysToRemove = implementedContracts.Keys.Where(k => k != attribute.ServiceContract.FullName).ToList();
            foreach (var key in keysToRemove)
            {
                implementedContracts.Remove(key);
            }
        }
    }
}
