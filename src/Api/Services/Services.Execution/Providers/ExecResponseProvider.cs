using Rhyous.EntityAnywhere.Services.Interfaces;
using Rhyous.EntityAnywhere.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhyous.EntityAnywhere.Services.Providers
{
    /// <summary>
    ///  A factory for creating a <see cref="ExecResponse"/>
    /// </summary>
    public sealed class ExecResponseProvider : IExecResponseProvider
    {
        #region Private fields

        private readonly IEnumerable<IExceptionHandler> _ExceptionHandlers;

        #endregion

        #region Constructor

        /// <summary>
        /// Construct a new <see cref="ExecResponseProvider"/> with the required Exception Handlers
        /// </summary>
        /// <param name="exceptionHandlers">The exception handlers responsible for doing something with exceptions</param>
        /// <exception cref="ArgumentNullException">Thrown when not providing exception handlers</exception>
        public ExecResponseProvider(IEnumerable<IExceptionHandler> exceptionHandlers)
        {
            _ExceptionHandlers = exceptionHandlers ?? throw new ArgumentNullException("You must provide a list of the exceptions you want to handle even if you don't intend to do anything with them");
        }

        #endregion

        #region ExecResponse non generic version

        /// <summary>
        /// Wraps code in a try catch wrap to enforce error logging
        /// </summary>
        /// <param name="code">The code to run that will be wrapped in a try catch</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public ExecResponse TryCatchWrap(Action code, int errorCode, string message)
        {
            try
            {
                code();
                return new ExecResponse(0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler(ex, errorCode, message);
            }
        }

        /// <summary>
        /// Wraps async code in a try catch wrap to enforce error logging
        /// </summary>
        /// <param name="code">The code to run that will be wrapped in a try catch</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse> AsyncTryCatchWrap(Func<Task> code, int errorCode, string message)
        {
            try
            {
                await code();
                return new ExecResponse(0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler(ex, errorCode, message);
            }
        }

        /// <summary>
        /// Wraps code in a try, catch, finally wrap to enforce error logging and to allow you to perform additional actions after the catch.
        /// </summary>
        /// <param name="code">The code to run that will be wrapped in a try catch</param><param name="code"></param>
        /// <param name="finallyCode">Code to be executed in the finally block</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public ExecResponse TryCatchFinallyWrap(Action code, Action finallyCode, int errorCode, string message)
        {
            try
            {
                code();
                return new ExecResponse(0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler(ex, errorCode, message);
            }
            finally
            {
                finallyCode();
            }
        }

        /// <summary>
        /// Wraps async code in a try, catch, finally wrap to enforce error logging and to allow you to perform additional actions after the catch.
        /// </summary>
        /// <param name="code">The code to run that will be wrapped in a try catch</param><param name="code"></param>
        /// <param name="finallyCode">Code to be executed in the finally block</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse> AsyncTryCatchFinallyWrap(Func<Task> code, Action finallyCode, int errorCode, string message)
        {
            try
            {
                await code();
                return new ExecResponse(0, "Success", true);
            }
            catch (Exception ex)
            {
                // Match the most specific exception
                var match = _ExceptionHandlers?.OrderByDescending(x => x.Severity).ToList().FirstOrDefault(x => x.ExceptionType == ex.GetType());

                // If there is no specific match to the exact type match to the next generic type
                if (match == null)
                    match = _ExceptionHandlers?.OrderByDescending(x => x.Severity).ToList().FirstOrDefault(x => x.ExceptionType.IsAssignableFrom(ex.GetType()));

                // If match is still null then we don't want to swallow the exception so we should just rethrow it
                if (match == null)
                    throw;

                // Handle the exception
                match.Handler(ex);

                // Return an ExecResponse
                var exceptionMessage = $"{message}: {ex.Message}";
                return new ExecResponse(errorCode, exceptionMessage);
            }
            finally
            {
                finallyCode();
            }
        }

        #endregion

        #region ExecResponse<T>

        /// <summary>
        /// Wraps code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="TResult">The type expected to be returned in <see cref="ExecResponse{T}.Value"/></typeparam>
        /// <param name="code">The code to be executed</param>
        /// <param name="errorCode">An error code to be returned in the case of failure</param>
        /// <param name="message">An error message to be concatenated with the exception message if an error occurs</param>
        /// <returns>An <see cref="ExecResponse{T}"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public ExecResponse<TResult> TryCatchWrap<TResult>(Func<TResult> code, int errorCode, string message)
        {
            try
            {
                var result = code();
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
        }

        /// <summary>
        /// Wraps code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="T1">The type of the first method parameter</typeparam>
        /// <typeparam name="TResult">The type expected to be returned in <see cref="ExecResponse{T}.Value"/></typeparam>
        /// <param name="code">The code to be executed</param>
        /// <param name="errorCode">An error code to be returned in the case of failure</param>
        /// <param name="message">An error message to be concatenated with the exception message if an error occurs</param>
        /// <returns>An <see cref="ExecResponse{T}"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public ExecResponse<TResult> TryCatchWrap<T1, TResult>(Func<T1, TResult> code, T1 input1, int errorCode, string message)
        {
            try
            {
                var result = code(input1);
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
        }


        /// <summary>
        /// Wraps code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="T1">The type of the first method parameter</typeparam>
        /// <typeparam name="T2">The type of the second method parameter</typeparam>
        /// <typeparam name="TResult">The type expected to be returned in <see cref="ExecResponse{T}.Value"/></typeparam>
        /// <param name="code">The code to be executed</param>
        /// <param name="errorCode">An error code to be returned in the case of failure</param>
        /// <param name="message">An error message to be concatenated with the exception message if an error occurs</param>
        /// <returns>An <see cref="ExecResponse{T}"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public ExecResponse<TResult> TryCatchWrap<T1, T2, TResult>(Func<T1, T2, TResult> code, T1 input1, T2 input2, int errorCode, string message)
        {
            try
            {
                var result = code(input1, input2);
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
        }

        /// <summary>
        /// Wraps code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="TResult">The type expected to be returned in <see cref="ExecResponse{T}.Value"/></typeparam>
        /// <param name="code">The code to be executed</param>
        /// <param name="errorCode">An error code to be returned in the case of failure</param>
        /// <param name="message">An error message to be concatenated with the exception message if an error occurs</param>
        /// <returns>An <see cref="ExecResponse{T}"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse<TResult>> TryCatchWrapAsync<TResult>(Func<Task<TResult>> code, int errorCode, string message)
        {
            try
            {
                var result = await code();
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
        }

        /// <summary>
        /// Wraps async code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="T1">The type of the first method parameter</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code">The code to run that will be wrapped in a try catch</param>
        /// <param name="input1">The first method parameter</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, TResult>(Func<T1, Task<TResult>> code, T1 input1, int errorCode, string message)
        {
            try
            {
                var result = await code(input1);
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
        }

        /// <summary>
        /// Wraps async code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="T1">The type of the first method parameter</typeparam>
        /// <typeparam name="T2">The type of the second method parameter</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code">The code to run that will be wrapped in a try catch</param>
        /// <param name="input1">The first method parameter</param>
        /// <param name="input2">The second method parameter</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, T2, TResult>(Func<T1, T2, Task<TResult>> code, T1 input1, T2 input2, int errorCode, string message)
        {
            try
            {
                var result = await code(input1, input2);
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
        }

        /// <summary>
        /// Wraps async code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="T1">The type of the first method parameter</typeparam>
        /// <typeparam name="T2">The type of the second method parameter</typeparam>
        /// <typeparam name="T3">The type of the third method parameter</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code">The code to run that will be wrapped in a try catch</param>
        /// <param name="input1">The first method parameter</param>
        /// <param name="input2">The second method parameter</param>
        /// <param name="input3">The third method parameter</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> code, T1 input1, T2 input2, T3 input3, int errorCode, string message)
        {
            return await TryCatchWrapAsync(code, input1, input2, input3, errorCode, message, "{0}: {1}");
        }

        /// <summary>
        /// Wraps async code in a try catch wrap to enforce error logging
        /// </summary>
        /// <typeparam name="T1">The type of the first method parameter</typeparam>
        /// <typeparam name="T2">The type of the second method parameter</typeparam>
        /// <typeparam name="T3">The type of the third method parameter</typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="code">The code to run that will be wrapped in a try catch</param>
        /// <param name="input1">The first method parameter</param>
        /// <param name="input2">The second method parameter</param>
        /// <param name="input3">The third method parameter</param>
        /// <param name="errorCode">An error code returned in the case of a failure</param>
        /// <param name="message">A message to be displayed</param>
        /// <returns>An <see cref="ExecResponse"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse<TResult>> TryCatchWrapAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> code, T1 input1, T2 input2, T3 input3, int errorCode, string message, string messageTemplate)
        {
            try
            {
                var result = await code(input1, input2, input3);
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message, messageTemplate);
            }
        }

        /// <summary>
        /// Wraps code in a try, catch, finally wrap to enforce error logging and to allow you to perform additional actions afterwards
        /// </summary>
        /// <typeparam name="TResult">The type expected to be returned in <see cref="ExecResponse{T}.Value"/></typeparam>
        /// <param name="code">The code to be executed</param>
        /// <param name="finallyCode">Code to be executed in the finally block</param>
        /// <param name="errorCode">An error code to be returned in the case of failure</param>
        /// <param name="message">An error message to be concatenated with the exception message if an error occurs</param>
        /// <returns>An <see cref="ExecResponse{TResult}"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public ExecResponse<TResult> TryCatchFinallyWrap<TResult>(Func<TResult> code, Action finallyCode, int errorCode, string message)
        {
            try
            {
                var result = code();
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
            finally
            {
                finallyCode();
            }
        }

        /// <summary>
        /// Wraps async code in a try, catch, finally wrap to enforce error logging and to allow you to perform additional actions afterwards
        /// </summary>
        /// <typeparam name="TResult">The type expected to be returned in <see cref="ExecResponse{T}.Value"/></typeparam>
        /// <param name="code">The code to be executed</param>
        /// <param name="finallyCode">Code to be executed in the finally block</param>
        /// <param name="errorCode">An error code to be returned in the case of failure</param>
        /// <param name="message">An error message to be concatenated with the exception message if an error occurs</param>
        /// <returns>An <see cref="ExecResponse{TResult}"/> with a success response or a failure response: <see cref="ExecResponse.Success"/></returns>
        public async Task<ExecResponse<TResult>> AsyncTryCatchFinallyWrap<TResult>(Func<Task<TResult>> code, Action finallyCode, int errorCode, string message)
        {
            try
            {
                var result = await code();
                return new ExecResponse<TResult>(result, 0, "Success", true);
            }
            catch (Exception ex)
            {
                return CatchHandler<TResult>(ex, errorCode, message);
            }
            finally
            {
                finallyCode();
            }
        }

        #endregion

        internal ExecResponse CatchHandler(Exception ex, int errorCode, string message)
        {
            string exceptionMessage = CreateMessage(ex, message);
            return new ExecResponse(errorCode, exceptionMessage);
        }

        internal ExecResponse<T> CatchHandler<T>(Exception ex, int errorCode, string message, string messageTemplate = null)
        {
            string exceptionMessage = CreateMessage(ex, message, messageTemplate);
            return new ExecResponse<T>(default(T), errorCode, exceptionMessage);
        }

        internal string CreateMessage(Exception ex, string message, string messageTemplate = null)
        {
            // Match the most specific exception
            var match = _ExceptionHandlers?.OrderByDescending(x => x.Severity).ToList().FirstOrDefault(x => x.ExceptionType == ex.GetType());

            // If there is no specific match to the exact type match to the next generic type
            if (match == null)
                match = _ExceptionHandlers?.OrderByDescending(x => x.Severity).ToList().FirstOrDefault(x => x.ExceptionType.IsAssignableFrom(ex.GetType()));

            // If match is still null then we don't want to swallow the exception so we should just rethrow it
            if (match == null)
                throw ex;

            // Handle the exception
            match.Handler(ex);

            // Return an ExecResponse
            var exceptionMessage = string.IsNullOrWhiteSpace(messageTemplate)
                                 ? $"{message}: {ex.Message}"
                                 : string.Format(messageTemplate, message, ex.Message);
            return exceptionMessage;
        }
    }
}