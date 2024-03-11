using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces.Tools;
using System;
using System.Reflection;

namespace Rhyous.EntityAnywhere.Interfaces.Common.Tests
{
    /// <summary>
    /// Summary description for TaskRunnerTests
    /// </summary>
    [TestClass]
    public class ExceptionUnwrapperTests
    {
        #region Unwrap
        [TestMethod]
        public void ExceptionUnwrapper_Unwrap_Test()
        {
            // Arrange
            var exception1 = new FakeException();
            var aggregateException1 = new AggregateException(exception1);

            // Act
            var result = aggregateException1.Unwrap();

            // Assert
            Assert.AreEqual(exception1, result);
        }

        [TestMethod]
        public void ExceptionUnwrapper_Unwrap_MultipleInnerExceptions_Test()
        {
            // Arrange
            var exception1 = new FakeException("1");
            var exception2 = new FakeException("2");
            var aggregateException1 = new AggregateException(exception1, exception2);

            // Act
            var result = aggregateException1.Unwrap();

            // Assert
            Assert.AreEqual(exception1, result);
        }

        [TestMethod]
        public void ExceptionUnwrapper_Unwrap_NestedAggregateException_Test()
        {
            // Arrange
            var exception1 = new FakeException("1");
            var exception2 = new FakeException("2");
            var aggregateException1 = new AggregateException(exception1, exception2);
            var exception3 = new FakeException("3");
            var exception4 = new FakeException("4");
            var aggregateException2 = new AggregateException(exception4, exception3);
            var aggregateException3 = new AggregateException(aggregateException1, aggregateException2);

            // Act
            var result = aggregateException3.Unwrap();

            // Assert
            Assert.AreEqual(exception1, result);
        }

        [TestMethod]
        public void ExceptionUnwrapper_Unwrap_NestedTargetInvocationException_Test()
        {
            // Arrange
            var exception1 = new FakeException("1");
            var aggregateException1 = new TargetInvocationException(exception1);
            var aggregateException2 = new TargetInvocationException(aggregateException1);

            // Act
            var result = aggregateException2.Unwrap();

            // Assert
            Assert.AreEqual(exception1, result);
        }
        #endregion

        #region HandleException<TException, TResult>(this Exception e, bool ignoreException = false)
        [TestMethod]
        public void ExceptionUnwrapper_HandleException_Test()
        {
            // Arrange
            var exception1 = new FakeException();
            var aggregateException1 = new AggregateException(exception1);

            // Act
            // Assert
            Assert.ThrowsException<FakeException>(() =>
            {
                ExceptionUnwrapper.HandleException<AggregateException, string>(aggregateException1, false);
            });
        }

        [TestMethod]
        public void ExceptionUnwrapper_HandleException_MultipleInnerExceptions_Test()
        {
            // Arrange
            var exception1 = new FakeException("1");
            var exception2 = new FakeException("2");
            var aggregateException1 = new AggregateException(exception1, exception2);

            // Act
            // Assert
            var e = Assert.ThrowsException<FakeException>(() =>
            {
                ExceptionUnwrapper.HandleException<AggregateException, string>(aggregateException1, false);
            });
            Assert.AreEqual(e.Message, "1");
        }

        [TestMethod]
        public void ExceptionUnwrapper_HandleException_NestedAggregateException_Test()
        {
            // Arrange
            var exception1 = new FakeException("1");
            var exception2 = new FakeException("2");
            var aggregateException1 = new AggregateException(exception1, exception2);
            var exception3 = new FakeException("3");
            var exception4 = new FakeException("4");
            var aggregateException2 = new AggregateException(exception4, exception3);
            var aggregateException3 = new AggregateException(aggregateException1, aggregateException2);

            // Act
            // Assert
            var e = Assert.ThrowsException<FakeException>(() =>
            {
                ExceptionUnwrapper.HandleException<AggregateException, string>(aggregateException3, false);
            });
            Assert.AreEqual(e.Message, "1");
        }

        [TestMethod]
        public void ExceptionUnwrapper_HandleException_NestedTargetInvocationException_Test()
        {
            // Arrange
            var exception1 = new FakeException("1");
            var aggregateException1 = new TargetInvocationException(exception1);
            var aggregateException2 = new TargetInvocationException(aggregateException1);

            // Act
            // Assert
            var e = Assert.ThrowsException<FakeException>(() =>
            {
                ExceptionUnwrapper.HandleException<TargetInvocationException, string>(aggregateException2, false);
            });
            Assert.AreEqual(e.Message, "1");
        }
        #endregion
    }
}
