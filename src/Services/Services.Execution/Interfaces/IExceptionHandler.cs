using System;

namespace Rhyous.WebFramework.Services.Interfaces
{
    /// <summary>
    /// Represents the tying together of handling a specific exception type with what
    /// you want to do when that exception occurs
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// The type of exception you want to catch
        /// </summary>
        Type ExceptionType { get; }

        /// <summary>
        /// A higher severity indicates that this should be handled first
        /// </summary>
        int Severity { get; }

        /// <summary>
        /// What you want to do when the exception is thrown
        /// </summary>
        Action<Exception> Handler { get; }
    }
}
