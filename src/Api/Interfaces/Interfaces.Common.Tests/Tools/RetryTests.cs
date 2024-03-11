using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Tools;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    [TestClass]
    public class RetryTests
    {
        const int LeeWay = 9; // Milliseconds to allow for variation in stopwatch.

        #pragma warning disable 1998
        #region
        [TestMethod]
        public async Task Retry_NoParams_NoReturn_DefaultParameters_NoExceptions_NullFunc()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => 
            {
                await new Retry().RetryAsync((Func<Task>)null, Retry.DefaultRetries, Retry.DefaultRetryDelay);
            });
        }

        [TestMethod]
        public async Task Retry_NoParams_NoReturn_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;

            // Act
            new Retry().RetryAsync(async () => { i++; }).Wait();

            // Assert
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public async Task Retry_NoParams_NoReturn_DefaultParameters_Exceptions()
        {
            // Arrange
            int i = 0;

            // Act
            new Retry().RetryAsync(async () => 
            {
                while (++i < Retry.DefaultRetries)
                    throw new Exception("Try again.");
                
            }).Wait();

            // Assert
            Assert.AreEqual(Retry.DefaultRetries, i);
        }

        [TestMethod]
        public async Task Retry_NoParams_NoReturn_ChangedRetryCount_Exceptions()
        {
            // Arrange
            int i = 0;
            int retryCount = 4;

            // Act
            new Retry().RetryAsync(async () =>
            {
                while (++i < retryCount)
                    throw new Exception("Try again.");

            }, retryCount).Wait();

            // Assert
            Assert.AreEqual(retryCount, i);
        }

        [TestMethod]
        public async Task Retry_NoParams_NoReturn_TestDelay_Exceptions()
        {
            // Arrange
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int i = 0;
            long[] times = new long[3];
            var retries = 3;

            // Act
            new Retry().RetryAsync(async () => {
                times[i++] = stopWatch.ElapsedMilliseconds;
                while (i < retries)
                    throw new Exception("Try again.");
            }, retries).Wait();

            var timeBetweenRetry1And2 = times[1] - times[0];
            var timeBetweenRetry2And3 = times[2] - times[1];

            // Assert
            Assert.IsTrue(timeBetweenRetry1And2 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[0]}. Second time: {times[1]}.");
            Assert.IsTrue(timeBetweenRetry2And3 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[1]}. Second time: {times[2]}.");
        }
        #endregion

        #region
        [TestMethod]
        public async Task Retry_NoParams_Return_DefaultParameters_NoExceptions_NullFunc()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => 
            {
                await new Retry().RetryAsync((Func<Task<int>>)null, Retry.DefaultRetries, Retry.DefaultRetryDelay);
            });
        }

        [TestMethod]
        public async Task Retry_NoParams_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;

            // Act
            var result = new Retry().RetryAsync(async () => { return i++; }).Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_NoParams_Return_DefaultParameters_Exceptions()
        {
            // Arrange
            int i = 0;

            // Act
            var actual = new Retry().RetryAsync(async () =>
            {
                while (++i < Retry.DefaultRetries)
                    throw new Exception("Try again.");
                return 100;
            }).Result;

            // Assert
            Assert.AreEqual(Retry.DefaultRetries, i);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_NoParams_Return_ChangedRetryCount_Exceptions()
        {
            // Arrange
            int i = 0;
            int retryCount = 4;

            // Act
            var actual = new Retry().RetryAsync(async () =>
            {
                while (++i < retryCount)
                    throw new Exception("Try again.");
                return 100;
            }, retryCount).Result;

            // Assert
            Assert.AreEqual(retryCount, i);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_NoParams_Return_TestDelay_Exceptions()
        {
            // Arrange
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int i = 0;
            long[] times = new long[3];
            int retries = 3;

            // Act
            var actual = new Retry().RetryAsync(async () => {
                times[i++] = stopWatch.ElapsedMilliseconds;
                while (i < retries)
                    throw new Exception("Try again.");
                return 100;
            }, retries).Result;

            var timeBetweenRetry1And2 = times[1] - times[0];
            var timeBetweenRetry2And3 = times[2] - times[1];

            // Assert
            Assert.AreEqual(100, actual);
            Assert.IsTrue(timeBetweenRetry1And2 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[0]}. Second time: {times[1]}.");
            Assert.IsTrue(timeBetweenRetry2And3 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[1]}. Second time: {times[2]}.");
        }
        #endregion

        #region One parameter
        [TestMethod]
        public async Task Retry_OneParams_Return_DefaultParameters_NoExceptions_NullFunc()
        {
           await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await new Retry().RetryAsync((Func<string, Task<int>>)null, "a", Retry.DefaultRetries, Retry.DefaultRetryDelay);
            });
        }

        [TestMethod]
        public async Task Retry_OneParamString_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            string param1 = null;

            // Act
            var result = new Retry().RetryAsync(async (string p1) => 
            {
                param1 = p1;
                return i++;
            }, "a").Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual("a", param1);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_OneParamInt_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;

            // Act
            var result = new Retry().RetryAsync(async (int p1) =>
            {
                param1 = p1;
                return i++;
            }, 27).Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_OneParam_Return_DefaultParameters_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;

            // Act
            var actual = new Retry().RetryAsync(async (int p1) =>
            {
                param1 = p1;
                while (++i < Retry.DefaultRetries)
                    throw new Exception("Try again.");
                return 100;
            }, 27).Result;

            // Assert
            Assert.AreEqual(Retry.DefaultRetries, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_OneParam_Return_ChangedRetryCount_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            int retryCount = 4;

            // Act
            var actual = new Retry().RetryAsync(async (int p1) =>
            {
                param1 = p1;
                while (++i < retryCount)
                    throw new Exception("Try again.");
                return 100;
            }, 27, retryCount).Result;

            // Assert
            Assert.AreEqual(retryCount, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_OneParam_Return_TestDelay_Exceptions()
        {
            // Arrange
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int i = 0;
            long[] times = new long[3];
            int retries = 3;

            // Act
            var actual = new Retry().RetryAsync(async (int p1) => {
                times[i++] = stopWatch.ElapsedMilliseconds;
                while (i < retries)
                    throw new Exception("Try again.");
                return 100;
            }, 27, retries).Result;

            var timeBetweenRetry1And2 = times[1] - times[0];
            var timeBetweenRetry2And3 = times[2] - times[1];

            // Assert
            Assert.AreEqual(100, actual);
            Assert.IsTrue(timeBetweenRetry1And2 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[0]}. Second time: {times[1]}.");
            Assert.IsTrue(timeBetweenRetry2And3 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[1]}. Second time: {times[2]}.");
        }
        #endregion

        #region Two parameters
        [TestMethod]
        public async Task Retry_TwoParams_Return_DefaultParameters_NoExceptions_NullFunc()
        {
           await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await new Retry().RetryAsync((Func<string,string,Task<int>>)null, "a", "b", Retry.DefaultRetries, Retry.DefaultRetryDelay);
            });
        }

        [TestMethod]
        public async Task Retry_TwoParamsString_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            string param1 = null;
            string param2 = null;

            // Act
            var result = new Retry().RetryAsync(async (string p1, string p2) =>
            {
                param1 = p1;
                param2 = p2;
                return i++;
            }, "a", "b").Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual("a", param1);
            Assert.AreEqual("b", param2);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_TwoParamsInt_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            int param2 = 0;

            // Act
            var result = new Retry().RetryAsync(async (int p1, int p2) =>
            {
                param1 = p1;
                param2 = p2;
                return i++;
            }, 27, 28).Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(28, param2);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_TwoParams_Return_DefaultParameters_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, int p2) =>
            {
                param1 = p1;
                while (++i < Retry.DefaultRetries)
                    throw new Exception("Try again.");
                return 100;
            }, 27, 28).Result;

            // Assert
            Assert.AreEqual(Retry.DefaultRetries, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_TwoParams_Return_ChangedRetryCount_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            int retryCount = 4;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, int p2) =>
            {
                param1 = p1;
                while (++i < retryCount)
                    throw new Exception("Try again.");
                return 100;
            }, 27, 28, retryCount).Result;

            // Assert
            Assert.AreEqual(retryCount, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_TwoParams_Return_TestDelay_Exceptions()
        {
            // Arrange
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int i = 0;
            long[] times = new long[3];
            int retries = 3;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, int p2) => {
                times[i++] = stopWatch.ElapsedMilliseconds;
                while (i < retries)
                    throw new Exception("Try again.");
                return 100;
            }, 27, 28, retries).Result;

            var timeBetweenRetry1And2 = times[1] - times[0];
            var timeBetweenRetry2And3 = times[2] - times[1];

            // Assert
            Assert.AreEqual(100, actual);
            Assert.IsTrue(timeBetweenRetry1And2 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[0]}. Second time: {times[1]}.");
            Assert.IsTrue(timeBetweenRetry2And3 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[1]}. Second time: {times[2]}.");
        }
        #endregion

        #region Three parameters
        [TestMethod]
        public async Task Retry_ThreeParams_Return_DefaultParameters_NoExceptions_NullFunc()
        {
           await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await new Retry().RetryAsync((Func<string, string, string, Task<int>>)null, "a", "b", "c", Retry.DefaultRetries, Retry.DefaultRetryDelay);
            });
        }

        [TestMethod]
        public async Task Retry_ThreeParamsString_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            string param1 = null;
            string param2 = null;
            string param3 = null;

            // Act
            var result = new Retry().RetryAsync(async (string p1, string p2, string p3) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                return i++;
            }, "a", "b", "c").Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual("a", param1);
            Assert.AreEqual("b", param2);
            Assert.AreEqual("c", param3);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_ThreeParamsInt_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;

            // Act
            var result = new Retry().RetryAsync(async (int p1, int p2, int p3) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                return i++;
            }, 27, 28, 29).Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(28, param2);
            Assert.AreEqual(29, param3);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_ThreeParams_Return_DefaultParameters_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            long param2 = 0;
            double param3 = 0;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, long p2, double p3) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                while (++i < Retry.DefaultRetries)
                    throw new Exception("Try again.");
                return 100;
            }, 27, 28, 29).Result;

            // Assert
            Assert.AreEqual(Retry.DefaultRetries, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(28, param2);
            Assert.AreEqual(29, param3);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_ThreeParams_Return_ChangedRetryCount_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            long param2 = 0;
            double param3 = 0;
            int retryCount = 4;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, long p2, double p3) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                while (++i < retryCount)
                    throw new Exception("Try again.");
                return 100;
            }, 27, 28, 29, retryCount).Result;

            // Assert
            Assert.AreEqual(retryCount, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(28, param2);
            Assert.AreEqual(29, param3);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_ThreeParams_Return_TestDelay_Exceptions()
        {
            // Arrange
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int i = 0;
            long[] times = new long[3];
            int retries = 3;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, object p2, string p3) => {
                times[i++] = stopWatch.ElapsedMilliseconds;
                while (i < retries)
                    throw new Exception("Try again.");
                return 100;
            }, 27, new object(), "a", retries).Result;

            var timeBetweenRetry1And2 = times[1] - times[0];
            var timeBetweenRetry2And3 = times[2] - times[1];

            // Assert
            Assert.AreEqual(100, actual);
            Assert.IsTrue(timeBetweenRetry1And2 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[0]}. Second time: {times[1]}.");
            Assert.IsTrue(timeBetweenRetry2And3 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[1]}. Second time: {times[2]}.");
        }
        #endregion
        
        #region Four parameters
        [TestMethod]
        public async Task Retry_FourParams_Return_DefaultParameters_NoExceptions_NullFunc()
        {
           await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await new Retry().RetryAsync((Func<string, string, string, string, Task<int>>)null, "a", "b", "c", "d", Retry.DefaultRetries, Retry.DefaultRetryDelay);
            });
        }


        [TestMethod]
        public async Task Retry_FourParamsString_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            string param1 = null;
            string param2 = null;
            string param3 = null;
            string param4 = null;

            // Act
            var result = new Retry().RetryAsync(async (string p1, string p2, string p3, string p4) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                param4 = p4;
                return i++;
            }, "a", "b", "c", "d").Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual("a", param1);
            Assert.AreEqual("b", param2);
            Assert.AreEqual("c", param3);
            Assert.AreEqual("d", param4);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_FourParamsInt_Return_DefaultParameters_NoExceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            int param2 = 0;
            int param3 = 0;
            int param4 = 0;

            // Act
            var result = new Retry().RetryAsync(async (int p1, int p2, int p3, int p4) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                param4 = p4;
                return i++;
            }, 27, 28, 29, 30).Result;

            // Assert
            Assert.AreEqual(1, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(28, param2);
            Assert.AreEqual(29, param3);
            Assert.AreEqual(30, param4);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public async Task Retry_FourParams_Return_DefaultParameters_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            long param2 = 0;
            double param3 = 0;
            object param4 = null;
            var obj = new object();

            // Act
            var actual = new Retry().RetryAsync(async (int p1, long p2, double p3, object p4) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                param4 = p4;
                while (++i < Retry.DefaultRetries)
                    throw new Exception("Try again.");
                return 100;
            }, 27, 28, 29, obj).Result;

            // Assert
            Assert.AreEqual(Retry.DefaultRetries, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(28, param2);
            Assert.AreEqual(29, param3);
            Assert.AreEqual(obj, param4);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_FourParams_Return_ChangedRetryCount_Exceptions()
        {
            // Arrange
            int i = 0;
            int param1 = 0;
            long param2 = 0;
            double param3 = 0;
            object param4 = null;
            var obj = new object();
            int retryCount = 4;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, long p2, double p3, object p4) =>
            {
                param1 = p1;
                param2 = p2;
                param3 = p3;
                param4 = p4;
                while (++i < retryCount)
                    throw new Exception("Try again.");
                return 100;
            }, 27, 28, 29, obj, retryCount).Result;

            // Assert
            Assert.AreEqual(retryCount, i);
            Assert.AreEqual(27, param1);
            Assert.AreEqual(28, param2);
            Assert.AreEqual(29, param3);
            Assert.AreEqual(obj, param4);
            Assert.AreEqual(100, actual);
        }

        [TestMethod]
        public async Task Retry_FourParams_Return_TestDelay_Exceptions()
        {
            // Arrange
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            int i = 0;
            long[] times = new long[3];
            var obj = new object();
            var guid = new Guid();
            var retries = 3;

            // Act
            var actual = new Retry().RetryAsync(async (int p1, object p2, string p3, Guid p4) => {
                times[i++] = stopWatch.ElapsedMilliseconds;
                while (i < retries)
                    throw new Exception("Try again.");
                return 100;
            }, 27, obj, "a", guid, retries).Result;

            var timeBetweenRetry1And2 = times[1] - times[0];
            var timeBetweenRetry2And3 = times[2] - times[1];

            // Assert
            Assert.AreEqual(100, actual);
            Assert.IsTrue(timeBetweenRetry1And2 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry1And2}. First time: {times[0]}. Second time: {times[1]}.");
            Assert.IsTrue(timeBetweenRetry2And3 >= Retry.DefaultRetryDelay - LeeWay, $"Time between is: {timeBetweenRetry2And3}. First time: {times[1]}. Second time: {times[2]}.");
        }
        #endregion
#pragma warning restore 1998
    }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
}