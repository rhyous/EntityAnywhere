using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhyous.WebFramework.Services.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;

namespace Services.Execution.Tests
{
    [TestClass]
    public class Theoretical
    {        
        [TestMethod]
        public void CanCatchANullReferenceException()
        {
            // Arrange
            bool doneSomething = false;

            // Act
            TryCatchWrap(() =>
            {
                throw new NullReferenceException();
            }, new NullReferenceHandler());

            // Assert
            Assert.IsFalse(doneSomething);
        }


        public void TryCatchWrap(Action somethingToDo, params IExceptionHandler[] exceptionHandlers)
        {
            try
            {
                somethingToDo();
            }
            catch (Exception ex)
            {
                // Match the most specific exception
                var match = exceptionHandlers?.OrderByDescending(x => x.Severity).ToList().FirstOrDefault(x => x.ExceptionType == ex.GetType());

                // If there is no specific match to the exact type match to the next generic type
                if (match == null)
                    match = exceptionHandlers?.OrderByDescending(x => x.Severity).ToList().FirstOrDefault(x => x.ExceptionType.IsAssignableFrom(ex.GetType()));

                // If match is still null then we don't want to swallow the exception so we should just rethrow it
                if (match == null)
                    throw;

                // Handle the exception
                match.Handler(ex);
            }
        }

        /// <summary>
        /// Handles a Null reference exception
        /// </summary>
        /// <param name="nullReferenceException"></param>
        public void NullReferenceHandler(Exception nullReferenceException)
        {
            var ex = nullReferenceException as NullReferenceException;

            Debug.WriteLine(ex.Message);
        }
    }




    public class NullReferenceHandler : IExceptionHandler
    {
        public Type ExceptionType => typeof(NullReferenceException);

        public int Severity => 3;

        public Action<Exception> Handler => (ex) => {
            Debug.WriteLine(ex.Message);
        };
    }
}
