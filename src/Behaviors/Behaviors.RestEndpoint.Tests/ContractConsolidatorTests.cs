using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Attributes;
using Rhyous.WebFramework.Behaviors;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Rhyous.EntityAnywhere.Behaviors.RestEndpoint.Tests
{
    [TestClass]
    public class ContractConsolidatorTests
    {
        [ServiceContract]
        interface Contract1 { }

        [ServiceContract]
        interface Contract2 : Contract1 { }


        [TestMethod]
        public void ConsolidateByInheritanceTests()
        {
            // Arrange
            var c1 = ContractDescription.GetContract(typeof(Contract1));
            var c2 = ContractDescription.GetContract(typeof(Contract2));
            IDictionary<string, ContractDescription> contracts = new Dictionary<string, ContractDescription>
            {
                { typeof(Contract1).FullName, c1 },
                { typeof(Contract2).FullName, c2 }
            };
            // Act
            ContractConsolidator.ConsolidateByInheritance(contracts);

            //Assert
            Assert.AreEqual(typeof(Contract2).FullName, contracts.Keys.Single());
            Assert.AreEqual(c2, contracts.Values.Single());
        }

        [TestMethod]
        public void ConsolidateByAttributeTests()
        {
            // Arrange
            var attribute = new CustomWebServiceAttribute { ServiceContract = typeof(Contract1) };
            var c1 = ContractDescription.GetContract(typeof(Contract1));
            var c2 = ContractDescription.GetContract(typeof(Contract2));
            IDictionary<string, ContractDescription> contracts = new Dictionary<string, ContractDescription>
            {
                { typeof(Contract1).FullName, c1 },
                { typeof(Contract2).FullName, c2 }
            };
            // Act
            ContractConsolidator.ConsolidateByAttribute(attribute, contracts);

            //Assert
            Assert.AreEqual(typeof(Contract1).FullName, contracts.Keys.Single());
            Assert.AreEqual(c1, contracts.Values.Single());
        }
    }
}
