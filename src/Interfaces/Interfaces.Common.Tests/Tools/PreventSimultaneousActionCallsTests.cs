using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    [TestClass]
    public class PreventSimultaneousActionCallsTests
    {
        #region PreventSimultaneousActionCalls
        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls();
            var callCount = 0;
            Action method = () =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls();
            var callCount = 0;
            Action method = () =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls();
            var callCount = 0;
            Action method = () =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls();
            var callCount = 0;
            Action method = () =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousActionCalls 1 param
        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_OnlyOneSimultaneousCallAllowed_1Param()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int>();
            var callCount = 0;
            Action<int> method = (int i) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_OnlyOneSimultaneousCallAllowed_1Param_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int>();
            var callCount = 0;
            Action<int> method = (int i) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_OnlyOneSimultaneousCallAllowed_1Param()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int>();
            var callCount = 0;
            Action<int> method = (int i) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_OnlyOneSimultaneousCallAllowed_1Param_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int>();
            var callCount = 0;
            Action<int> method = (int i) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousActionCalls 2 param
        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_2Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string>();
            var callCount = 0;
            Action<int, string> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, ""))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_2Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string>();
            var callCount = 0;
            Action<int, string> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, ""))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, ""))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_2Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string>();
            var callCount = 0;
            Action<int, string> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, ""))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_2Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string>();
            var callCount = 0;
            Action<int, string> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, ""))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, ""))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousActionCalls 3 param
        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_3Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool>();
            var callCount = 0;
            Action<int, string, bool> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_3Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool>();
            var callCount = 0;
            Action<int, string, bool> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_3Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool>();
            var callCount = 0;
            Action<int, string, bool> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_3Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool>();
            var callCount = 0;
            Action<int, string, bool> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousActionCalls 4 param
        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_4Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object>();
            var callCount = 0;
            Action<int, string, bool, object> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_4Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object>();
            var callCount = 0;
            Action<int, string, bool, object> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_4Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object>();
            var callCount = 0;
            Action<int, string, bool, object> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(100);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_4Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object>();
            var callCount = 0;
            Action<int, string, bool, object> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousActionCalls 5 param
        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_5Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object, char>();
            var callCount = 0;
            Action<int, string, bool, object, char> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_Call_5Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object, char>();
            var callCount = 0;
            Action<int, string, bool, object, char> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.Call(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_5Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object, char>();
            var callCount = 0;
            Action<int, string, bool, object, char> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousActionCalls_CallAsync_5Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousActionCalls = new PreventSimultaneousActionCalls<int, string, bool, object, char>();
            var callCount = 0;
            Action<int, string, bool, object, char> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousActionCalls.CallAsync(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousFuncCalls
        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int>();
            var callCount = 0;
            Func<int> method = () =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int>();
            var callCount = 0;
            Func<int> method = () =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int>();
            var callCount = 0;
            Func<int> method = () =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int>();
            var callCount = 0;
            Func<int> method = () =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousFuncCalls 1 param
        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_OnlyOneSimultaneousCallAllowed_1Param()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, int>();
            var callCount = 0;
            Func<int, int> method = (int i) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_OnlyOneSimultaneousCallAllowed_1Param_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, int>();
            var callCount = 0;
            Func<int, int> method = (int i) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_OnlyOneSimultaneousCallAllowed_1Param()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, int>();
            var callCount = 0;
            Func<int, int> method = (int i) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_OnlyOneSimultaneousCallAllowed_1Param_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, int>();
            var callCount = 0;
            Func<int, int> method = (int i) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousFuncCalls 2 param
        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_2Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, int>();
            var callCount = 0;
            Func<int, string, int> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, ""))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_2Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, int>();
            var callCount = 0;
            Func<int, string, int> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, ""))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, ""))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_2Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, int>();
            var callCount = 0;
            Func<int, string, int> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, ""))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_2Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, int>();
            var callCount = 0;
            Func<int, string, int> method = (int i, string s) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, ""))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "")),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, ""))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousFuncCalls 3 param
        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_3Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, int>();
            var callCount = 0;
            Func<int, string, bool, int> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_3Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, int>();
            var callCount = 0;
            Func<int, string, bool, int> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_3Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, int>();
            var callCount = 0;
            Func<int, string, bool, int> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_3Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, int>();
            var callCount = 0;
            Func<int, string, bool, int> method = (int i, string s, bool b) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false)),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousFuncCalls 4 param
        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_4Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, int>();
            var callCount = 0;
            Func<int, string, bool, object, int> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_4Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, int>();
            var callCount = 0;
            Func<int, string, bool, object, int> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_4Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, int>();
            var callCount = 0;
            Func<int, string, bool, object, int> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_4Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, int>();
            var callCount = 0;
            Func<int, string, bool, object, int> method = (int i, string s, bool b, object o) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object())),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object()))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion

        #region PreventSimultaneousFuncCalls 5 param
        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_5Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, char, int>();
            var callCount = 0;
            Func<int, string, bool, object, char, int> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_Call_5Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, char, int>();
            var callCount = 0;
            Func<int, string, bool, object, char, int> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.Call(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_5Param_OnlyOneSimultaneousCallAllowed()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, char, int>();
            var callCount = 0;
            Func<int, string, bool, object, char, int> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks);

            // Assert
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PreventSimultaneousFuncCalls_CallAsync_5Param_OnlyOneSimultaneousCallAllowed_2()
        {
            // Arrange
            var preventSimultaneousFuncCalls = new PreventSimultaneousFuncCalls<int, string, bool, object, char, int>();
            var callCount = 0;
            Func<int, string, bool, object, char, int> method = (int i, string s, bool b, object o, char c) =>
            {
                Thread.Sleep(75);
                return callCount++;
            };

            // Act
            var tasks1 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks1);

            var tasks2 = new[]
            {
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a')),
                Task.Run(() => preventSimultaneousFuncCalls.CallAsync(method, 0, "", false, new object(), 'a'))
            };
            await Task.WhenAll(tasks2);

            // Assert
            Assert.AreEqual(2, callCount);
        }

        #endregion
    }
}