using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.Odata;
using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using Rhyous.WebFramework.Services;
using Rhyous.WebFramework.Services.RelatedEntities;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.WebServices.Tests
{
    [TestClass]
    public class UserWebServiceTests
    {
        private MockRepository _MockRepository;

        private Mock<IUserRestHandlerProvider> _MockRestHandlerProvider;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockRestHandlerProvider = new Mock<IUserRestHandlerProvider>();
        }

        private UserWebService GetWebService()
        {
            return new UserWebService(_MockRestHandlerProvider.Object);
        }

        
    }
}