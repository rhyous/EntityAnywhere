using Rhyous.WebFramework.Services.Interfaces;
using System;

namespace Rhyous.WebFramework.Services.Models
{
    /// <summary>
    /// Represents a concrete <see cref="IExceptionHandler"/>
    /// </summary>
    public class ExceptionHandler : IExceptionHandler
    {
        /// <summary>
        /// Construct a new <see cref="ExceptionHandler"/> with the required parameters
        /// </summary>
        /// <param name="exceptionType">The specific exception type</param>
        /// <param name="handler">What should happen if that exception is encountered</param>
        /// <param name="severity">The severity of the exception. A higher severity handler will be handled before more generic handlers</param>
        public ExceptionHandler(Type exceptionType, Action<Exception> handler, int severity)
        {
            ExceptionType = exceptionType;
            Handler = handler;
            Severity = severity;
        }

        /// <summary>
        /// The type of exception you want to catch
        /// </summary>
        public Type ExceptionType { get; }

        /// <summary>
        /// A higher severity indicates that this should be handled first
        /// </summary>
        public int Severity { get; }

        /// <summary>
        /// What you want to do when the exception is thrown
        /// </summary>
        public Action<Exception> Handler { get; }
    }
}
