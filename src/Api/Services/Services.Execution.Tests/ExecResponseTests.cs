using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.EntityAnywhere.Interfaces;
using Rhyous.EntityAnywhere.Services.Interfaces;
using Rhyous.EntityAnywhere.Services.Providers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Services.Execution.Tests
{
    [TestClass]
    public class ExecResponseProviderTests
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
        public void ExecResponseProvider_Must_Be_Given_A_List_Of_Handlers()
        {
            // Arrange

            // Act

            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ExecResponseProvider(null));
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchWrap_Can_Create_Success_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);

            // Act
            var response = provider.TryCatchWrap(() =>
            {
            }, 666, "This won't fail");

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("Success", response.Message);
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchWrap_Can_Create_Failed_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);

            // Act
            var response = provider.TryCatchWrap(() =>
            {
                throw new Exception("There was an error in the code");
            }, 666, "The code failed for the following reason");

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual(666, response.ErrorCode);
            Assert.AreEqual("The code failed for the following reason: There was an error in the code", response.Message);
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchWrapT_Can_Create_Success_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);

            var myVar = "This is my var";

            // Act
            var response = provider.TryCatchWrap(() => MyMethod(myVar)
            , 666, "This won't fail");

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("Success", response.Message);
            Assert.AreEqual("This is my var", response.Value);
        }

        public string MyMethod(string myString)
        {
            return myString;
        }


        [TestMethod]
        public void ExecResponseProvider_TryCatchWrapT_Can_Create_Failed_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);

            // Act
            var response = provider.TryCatchWrap<string>(() =>
            {
                throw new Exception("There was an error in the code");
            }, 666, "The code failed for the following reason");

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual(666, response.ErrorCode);
            Assert.AreEqual("The code failed for the following reason: There was an error in the code", response.Message);
            Assert.IsNull(response.Value);
        }


        [TestMethod]
        public void ExecResponseProvider_TryCatchFinallyWrap_Can_Create_Success_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);
            bool finallyHasBeenHit = false;

            // Act
            var response = provider.TryCatchFinallyWrap(() =>
            {
            }, () =>
            {
                finallyHasBeenHit = true;
            }, 666, "This won't fail");

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("Success", response.Message);
            Assert.IsTrue(finallyHasBeenHit);
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchFinallyWrap_Can_Create_Failed_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);
            bool finallyHasBeenHit = false;

            // Act
            var response = provider.TryCatchFinallyWrap(() =>
            {
                throw new Exception("There was an error in the code");
            }, () =>
            {
                finallyHasBeenHit = true;
            }, 666, "The code failed for the following reason");

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual(666, response.ErrorCode);
            Assert.AreEqual("The code failed for the following reason: There was an error in the code", response.Message);
            Assert.IsTrue(finallyHasBeenHit);
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchFinallyWrapT_Can_Create_Success_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);
            bool finallyHasBeenHit = false;

            // Act
            var response = provider.TryCatchFinallyWrap(() =>
            {
                return "Code has worked";
            }, () =>
            {
                finallyHasBeenHit = true;
            }, 666, "This won't fail");

            // Assert
            Assert.IsTrue(response.Success);
            Assert.AreEqual(0, response.ErrorCode);
            Assert.AreEqual("Success", response.Message);
            Assert.AreEqual("Code has worked", response.Value);
            Assert.IsTrue(finallyHasBeenHit);
        }

        [TestMethod]
        public void ExecResponseProvider_TryCatchFinallyWrapT_Can_Create_Failed_Response()
        {
            // Arrange
            var provider = new ExecResponseProvider(handlers);
            bool finallyHasBeenHit = false;

            // Act
            var response = provider.TryCatchFinallyWrap<string>(() =>
            {
                throw new Exception("There was an error in the code");
            }, () =>
            {
                finallyHasBeenHit = true;
            }, 666, "The code failed for the following reason");

            // Assert
            Assert.IsFalse(response.Success);
            Assert.AreEqual(666, response.ErrorCode);
            Assert.AreEqual("The code failed for the following reason: There was an error in the code", response.Message);
            Assert.IsNull(response.Value);
            Assert.IsTrue(finallyHasBeenHit);
        }

    }

    class ExceptionHandler : IExceptionHandler
    {
        public Type ExceptionType => typeof(Exception);

        public int Severity => 1;

        public Action<Exception> Handler => (ex) => { };
    }


    class DebugLogger : ILogger
    {
        public void Debug(string msg, [CallerMemberName] string callingMethod = "", [CallerFilePath] string callingFilePath = "", [CallerLineNumber] int callingFileLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Write(string msg, [CallerMemberName] string callingMethod = "", [CallerFilePath] string callingFilePath = "", [CallerLineNumber] int callingFileLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public void Write(Exception msg, [CallerMemberName] string callingMethod = "", [CallerFilePath] string callingFilePath = "", [CallerLineNumber] int callingFileLineNumber = 0)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
