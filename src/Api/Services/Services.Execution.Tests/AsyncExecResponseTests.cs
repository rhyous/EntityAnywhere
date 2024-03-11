using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Services.Interfaces;
using Rhyous.EntityAnywhere.Services.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Execution.Tests
{
    [TestClass]
    public class AsyncExecResponseTests
    {
        private IEnumerable<IExceptionHandler> handlers;

        [TestInitialize]
        public void Init()
        {
            handlers = new List<IExceptionHandler>()
            {
                new ExceptionHandler()
            };
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchWrapAsync_Can_Create_Success_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);

            // Act
            var response = provider.AsyncTryCatchWrap(AnAsyncMethod, 666, "This won't fail").Result;

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("Success", response.Message);
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchWrapAsyncT_Can_Create_Success_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);

            // Act
            var response = provider.TryCatchWrapAsync(AnAsyncFunc, 666, "This won't fail").Result;

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("Success", response.Message);
            Assert.AreEqual("I was successful", response.Value);
        }

        private async Task AnAsyncMethod()
        {
            await Task.Run(() => { });
        }

        private async Task<string> AnAsyncFunc()
        {
            return await Task.Run(() => "I was successful");
        }

    }
}
