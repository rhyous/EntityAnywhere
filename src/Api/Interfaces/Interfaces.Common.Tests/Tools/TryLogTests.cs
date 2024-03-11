using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    /// <summary>
    /// Summary description for TryLogTests
    /// </summary>
    [TestClass]
    public class TryLogTests
    {
        #region Try Action Exception
        [TestMethod]
        public void TryLog_Action_0params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;

            // Act
            tryLog.Try(() => 
            {
                wasCalled = true;
                throw new Exception();
            });

            // Assert
            Assert.IsTrue(wasCalled);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Action_1param_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;

            // Act
            tryLog.Try((int i) =>
            {
                param1 = i;
                wasCalled = true;
                throw new Exception();
            }, 21);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Action_2params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;

            // Act
            tryLog.Try((int i1, int i2) =>
            {
                param1 = i1;
                param2 = i2;
                wasCalled = true;
                throw new Exception();
            }, 21, 22);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Action_3params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;

            // Act
            tryLog.Try((int i1, int i2, int i3) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                wasCalled = true;
                throw new Exception();
            }, 21, 22, 23);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Action_4params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;

            // Act
            tryLog.Try((int i1, int i2, int i3, int i4) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                param4 = i4;
                wasCalled = true;
                throw new Exception();
            }, 21, 22, 23, 24);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Action_5params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            int param5 = 0;

            // Act
            tryLog.Try((int i1, int i2, int i3, int i4, int i5) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                param4 = i4;
                param5 = i5;
                wasCalled = true;
                throw new Exception();
            }, 21, 22, 23, 24, 25);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            Assert.AreEqual(25, param5);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }
        #endregion
        
        #region Try Func Exception
        [TestMethod]
        public void TryLog_Func_0params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            Func<string> func = () =>
            {
                wasCalled = true;
                throw new Exception();
            };

            // Act
            var actual = tryLog.Try(func);

            // Assert
            Assert.IsTrue(wasCalled);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Func_1param_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            Func<int, string> func = (int i) =>
            {
                param1 = i;
                wasCalled = true;
                throw new Exception();
            };

            // Act
            tryLog.Try(func, 21);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Func_2params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            Func<int, int, string> func = (int i1, int i2) =>
            {
                param1 = i1;
                param2 = i2;
                wasCalled = true;
                throw new Exception();
            };

            // Act
            tryLog.Try(func, 21, 22);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Func_3params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            Func<int, int, int, string> func = (int i1, int i2, int i3) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                wasCalled = true;
                throw new Exception();
            };

            // Act
            tryLog.Try(func, 21, 22, 23);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Func_4params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            Func<int, int, int, int, string> func = 
                (int i1, int i2, int i3, int i4) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                param4 = i4;
                wasCalled = true;
                throw new Exception();
            };

            // Act
            tryLog.Try(func, 21, 22, 23, 24);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void TryLog_Func_5params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            int param5 = 0;
            Func<int, int, int, int, int, string> func =
                (int i1, int i2, int i3, int i4, int i5) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                param4 = i4;
                param5 = i5;
                wasCalled = true;
                throw new Exception();
            };
            // Act
            tryLog.Try(func, 21, 22, 23, 24, 25);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            Assert.AreEqual(25, param5);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region Try Func return
        [TestMethod]
        public void TryLog_Func_Returns_0params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            Func<string> func = () =>
            {
                wasCalled = true;
                return "val1";
            };

            // Act
            var actual = tryLog.Try(func);

            // Assert
            Assert.AreEqual("val1", actual);
            Assert.IsTrue(wasCalled);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TryLog_Func_Returns_1param_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            Func<int, string> func = (int i) =>
            {
                param1 = i;
                wasCalled = true;
                return "val1";
            };

            // Act
            var actual = tryLog.Try(func, 21);

            // Assert
            Assert.AreEqual("val1", actual);
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TryLog_Func_Returns_2params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            Func<int, int, string> func = (int i1, int i2) =>
            {
                param1 = i1;
                param2 = i2;
                wasCalled = true;
                return "val1";
            };

            // Act
            var actual = tryLog.Try(func, 21, 22);

            // Assert
            Assert.AreEqual("val1", actual);
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TryLog_Func_Returns_3params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            Func<int, int, int, string> func = (int i1, int i2, int i3) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                wasCalled = true;
                return "val1";
            };

            // Act
            var actual = tryLog.Try(func, 21, 22, 23);

            // Assert
            Assert.AreEqual("val1", actual);
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TryLog_Func_Returns_4params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            Func<int, int, int, int, string> func =
                (int i1, int i2, int i3, int i4) =>
                {
                    param1 = i1;
                    param2 = i2;
                    param3 = i3;
                    param4 = i4;
                    wasCalled = true;
                    return "val1";
                };

            // Act
            var actual = tryLog.Try(func, 21, 22, 23, 24);

            // Assert
            Assert.AreEqual("val1", actual);
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TryLog_Func_Returns_5params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            int param5 = 0;
            Func<int, int, int, int, int, string> func =
                (int i1, int i2, int i3, int i4, int i5) =>
                {
                    param1 = i1;
                    param2 = i2;
                    param3 = i3;
                    param4 = i4;
                    param5 = i5;
                    wasCalled = true;
                    return "val1";
                };
            // Act
            var actual = tryLog.Try(func, 21, 22, 23, 24, 25);

            // Assert
            Assert.AreEqual("val1", actual);
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            Assert.AreEqual(25, param5);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
        #endregion

        #region Try async Func Exception
        [TestMethod]
        public async Task TryLog_Func_Async_0params_Test() 
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            Func<Task<Task<string>>> func = () =>
            {
                wasCalled = true;
                throw new Exception();
            };

            // Act
            var actual = await tryLog.TryAsync(func);

            // Assert
            Assert.IsTrue(wasCalled);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task TryLog_Func_Async_1param_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            Func<int, Task<string>> func = (int i) =>
            {
                param1 = i;
                wasCalled = true;
                throw new Exception();
            };

            // Act
            var actual = await tryLog.TryAsync(func, 21);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task TryLog_Func_Async_2params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            Func<int, int, Task<string>> func = (int i1, int i2) =>
            {
                param1 = i1;
                param2 = i2;
                wasCalled = true;
                throw new Exception();
            };

            // Act
            var actual = await tryLog.TryAsync(func, 21, 22);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task TryLog_Func_Async_3params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            Func<int, int, int, Task<string>> func = (int i1, int i2, int i3) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                wasCalled = true;
                throw new Exception();
            };

            // Act
            var actual = await tryLog.TryAsync(func, 21, 22, 23);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task TryLog_Func_Async_4params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            Func<int, int, int, int, Task<string>> func =
                (int i1, int i2, int i3, int i4) =>
                {
                    param1 = i1;
                    param2 = i2;
                    param3 = i3;
                    param4 = i4;
                    wasCalled = true;
                    throw new Exception();
                };

            // Act
            var actual = await tryLog.TryAsync(func, 21, 22, 23, 24);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public async Task TryLog_Func_Async_5params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            int param5 = 0;
            Func<int, int, int, int, int, Task<string>> func =
                (int i1, int i2, int i3, int i4, int i5) =>
                {
                    param1 = i1;
                    param2 = i2;
                    param3 = i3;
                    param4 = i4;
                    param5 = i5;
                    wasCalled = true;
                    throw new Exception();
                };
            // Act
            var actual = await tryLog.TryAsync(func, 21, 22, 23, 24, 25);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            Assert.AreEqual(25, param5);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }
        #endregion

        #region Try async Func Return
        [TestMethod]
        public async Task TryLog_Func_Returns_Async_0params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            Func<Task<string>> func = async () =>
            {
                wasCalled = true;
                return await Task.FromResult("async_value");
            };

            // Act
            var actual = await tryLog.TryAsync(func);

            // Assert
            Assert.IsTrue(wasCalled);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task TryLog_Func_Returns_Async_1param_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            Func<int, Task<string>> func = async (int i) =>
            {
                param1 = i;
                wasCalled = true;
                return await Task.FromResult("async_value");
            };

            // Act
            var actual = await tryLog.TryAsync(func, 21);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task TryLog_Func_Returns_Async_2params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            Func<int, int, Task<string>> func = async (int i1, int i2) =>
            {
                param1 = i1;
                param2 = i2;
                wasCalled = true;
                return await Task.FromResult("async_value");
            };

            // Act
            var actual = await tryLog.TryAsync(func, 21, 22);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task TryLog_Func_Returns_Async_3params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            Func<int, int, int, Task<string>> func = async (int i1, int i2, int i3) =>
            {
                param1 = i1;
                param2 = i2;
                param3 = i3;
                wasCalled = true;
                return await Task.FromResult("async_value");
            };

            // Act
            var actual = await tryLog.TryAsync(func, 21, 22, 23);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task TryLog_Func_Returns_Async_4params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            Func<int, int, int, int, Task<string>> func =
                async (int i1, int i2, int i3, int i4) =>
                {
                    param1 = i1;
                    param2 = i2;
                    param3 = i3;
                    param4 = i4;
                    wasCalled = true;
                    return await Task.FromResult("async_value");
                };

            // Act
            var actual = await tryLog.TryAsync(func, 21, 22, 23, 24);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task TryLog_Func_Returns_Async_5params_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger>();
            mockLogger.Setup(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()));
            var tryLog = new TryLog(mockLogger.Object);
            bool wasCalled = false;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;
            int param5 = 0;
            Func<int, int, int, int, int, Task<string>> func =
                async (int i1, int i2, int i3, int i4, int i5) =>
                {
                    param1 = i1;
                    param2 = i2;
                    param3 = i3;
                    param4 = i4;
                    param5 = i5;
                    wasCalled = true;
                    return await Task.FromResult("async_value");
                };
            // Act
            var actual = await tryLog.TryAsync(func, 21, 22, 23, 24, 25);

            // Assert
            Assert.IsTrue(wasCalled);
            Assert.AreEqual(21, param1);
            Assert.AreEqual(22, param2);
            Assert.AreEqual(23, param3);
            Assert.AreEqual(24, param4);
            Assert.AreEqual(25, param5);
            mockLogger.Verify(m => m.Write(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
        #endregion 
    }
}
