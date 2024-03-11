using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class MethodInfoExtensionsTests
    {
        private MockRepository _MockRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            _MockRepository = new MockRepository(MockBehavior.Strict);

        }

        public async Task<string> ExampleMethodAsync(int i) 
        {
            await Task.CompletedTask;
            return $"Example {i}";
        }

        #region InvokeAsync
        [TestMethod]
        public async Task MethodInfoExtensions_InvokeAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            MethodInfo mi = GetType().GetMethod(nameof(ExampleMethodAsync));
            object obj = this;

            // Act
            var result = await mi.InvokeAsync<string>(obj, 27);

            // Assert
            Assert.AreEqual("Example 27", result);
            _MockRepository.VerifyAll();
        }
        #endregion
    }
}
