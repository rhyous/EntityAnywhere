using System;
using System.Runtime.ExceptionServices;

namespace Rhyous.EntityAnywhere.Interfaces.Tools
{
    /// <summary>A tool for unwrapping exceptions to get to the innermost exception message.</summary>
    public static class ExceptionUnwrapper
    {
        /// <summary>UnWraps an exception. It supports nested exceptions but does NOT support multiple
        /// inner exceptiosn at the same level.</summary>
        /// <param name="e">The exception.</param>
        /// <returns>The innermost exception.</returns>
        public static Exception Unwrap(this Exception e)
        {
            var currentEx = e;
            var innerEx = e.InnerException;
            while (innerEx != null)
            {
                currentEx = innerEx;
                innerEx = currentEx.InnerException;
            }
            return currentEx;
        }

        /// <summary>Used to handle a specific exception type, such as <see cref="AggregateException"/>, get the inner
        /// exeption and throw that instead.</summary>
        /// <typeparam name="TException">The exception to handle.</typeparam>
        /// <typeparam name="TResult">The result type to return. This is usually the parent method's return value.</typeparam>
        /// <param name="e">The exception.</param>
        /// <param name="ignoreException">Whether to ignore the exception or throw it. If ture, the default <see cref="TResult"/> is returned.</param>
        /// <returns></returns>
        public static TResult HandleException<TException, TResult>(this Exception e, bool ignoreException = false)
            where TException : Exception
        {
            if (ignoreException)
                return default(TResult);

            if (e is TException && e.InnerException != null)
            {
                var unwrappedEx = e.Unwrap();
                ExceptionDispatchInfo.Capture(unwrappedEx).Throw();
            }
            throw e;
        }

        /// <summary>Used to handle a specific exception type, such as <see cref="AggregateException"/>, get the inner
        /// exeption and throw that instead.</summary>
        /// <typeparam name="TException">The exception to handle.</typeparam>
        /// <param name="e">The exception.</param>
        /// <param name="ignoreException">Whether to ignore the exception or throw it.</param>
        /// <returns></returns>

        public static void HandleException<TException>(this Exception e, bool ignoreException = false)
            where TException : Exception
        {
            if (ignoreException)
                return;

            if (e is TException && e.InnerException != null)
            {
                var unwrappedEx = e.Unwrap();
                ExceptionDispatchInfo.Capture(unwrappedEx).Throw();
            }
            throw e;
        }
    }
}
