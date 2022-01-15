using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Rhyous.UnitTesting;
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
    public class FilterHandlerTests
    {
        private MockRepository _MockRepository;

        private Mock<ILifetimeScope> _MockLifetimeScope;
        private Mock<IRelatedEntityProvider<User, IUser, long>> _MockRelatedEntityProvider;
        private Mock<IUrlParameters> _MockUrlParameters;
        private Mock<IUserService> _MockUserService;
        private Mock<IRequestUri> _MockRequestUri;

        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

            _MockLifetimeScope = _MockRepository.Create<ILifetimeScope>();
            _MockRelatedEntityProvider = _MockRepository.Create<IRelatedEntityProvider<User, IUser, long>>();
            _MockUrlParameters = _MockRepository.Create<IUrlParameters>();
            _MockUserService = _MockRepository.Create<IUserService>();
            _MockRequestUri = _MockRepository.Create<IRequestUri>();
        }

        private FilterHandler CreateFilterHandler()
        {
            return new FilterHandler(
                _MockLifetimeScope.Object,
                _MockRelatedEntityProvider.Object,
                _MockUrlParameters.Object,
                _MockUserService.Object,
                _MockRequestUri.Object);
        }

        #region Filter
        [TestMethod]
        [ObjectNullOrNew(typeof(NameValueCollection))]
        public async Task FilterHandler_FilterEntitlements_Null_Test(NameValueCollection nvc)
        {
            // Arrange
            var webservice = CreateFilterHandler();
            _MockUrlParameters.Setup(m => m.Collection).Returns(nvc);

            // Act
            var actual = await webservice.FilterAsync();

            // Assert
            Assert.IsNull(actual);
            _MockRepository.VerifyAll();
        }

        [TestMethod]
        public async Task FilterHandler_FilterEntitlements_HasFilter_Test()
        {
            // Arrange
            var collection = new NameValueCollection
            {
                { "$filter", "contains(Id, '1')" },
                { "$expand", "Organization" },
                { "$top", "10" },
                { "$skip", "0" }
            };
            _MockUrlParameters.Setup(m => m.Collection).Returns(collection);

            var expectedFilter = "contains(Id, '1')";
            string actualFilter = "";

            var expectedCollection = new NameValueCollection
            {
                { "$filter", "contains(Id, '1')" },
                { "$expand", "Organization" },
                { "$top", "10" },
                { "$skip", "0" }
            };
            NameValueCollection actualCollection = null;

            _MockUserService.Setup(m => m.FilterUsersAsync(It.IsAny<string>(), It.IsAny<NameValueCollection>()))
                            .ReturnsAsync((string f, NameValueCollection nvc) =>
                            {
                                actualFilter = f;
                                actualCollection = nvc;
                                return null;
                            });
            var uri = new Uri("https://site.domain.tld/api/UserService/Users/Filter");
            _MockRequestUri.Setup(m => m.Uri).Returns(uri);

            var webservice = CreateFilterHandler();

            // Act
            var actual = await webservice.FilterAsync();

            // Assert
            Assert.AreEqual(expectedFilter, actualFilter);
            var nvc1 = expectedCollection.AllKeys.ToDictionary(k => k, k => expectedCollection[k]);
            var nvc2 = actualCollection.AllKeys.ToDictionary(k => k, k => collection[k]);
            CollectionAssert.AreEquivalent(nvc1, nvc2);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
