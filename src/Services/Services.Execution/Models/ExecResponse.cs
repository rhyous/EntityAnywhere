using System;

namespace Rhyous.WebFramework.Services.Models
{
    /// <summary>
    /// Represents the response of a successful execution of a block of code. This type is immutable once created
    /// </summary>
    public class ExecResponse
    {
        /// <summary>
        /// Construct a new Exec response
        /// </summary>
        /// <param name="errorCode">An error code if applicable</param>
        /// <param name="message">A message</param>
        /// <param name="success">Was the execution successful</param>
        internal ExecResponse(int errorCode, string message, bool success = false)
        {
            ErrorCode = errorCode;
            Message = message;
            Success = success;            
        }

        /// <summary>
        /// An error code if applicable
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Was the execution successful
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// A message
        /// </summary>
        public string Message { get; private set; }

    }

    /// <summary>
    /// Extends <see cref="ExecResponse"/> to include the response of a block of code such as the return type of a <see cref="Func{T}"/>. Immutable once created
    /// </summary>
    /// <typeparam name="T">The expected response type</typeparam>
    public class ExecResponse<T> : ExecResponse
    {
        /// <summary>
        /// Construct  a new <see cref="ExecResponse{T}"/>
        /// </summary>
        /// <param name="value">The result of a <see cref="Func{TResult}"/></param>
        /// <param name="errorCode">An error code if applicable</param>
        /// <param name="message">A message</param>
        /// <param name="success">Was the execution successful</param>
        public ExecResponse(T value, int errorCode, string message, bool success = false) : base(errorCode, message, success)
        {
            Value = value;
        }

        /// <summary>
        /// The result of the execution of code
        /// </summary>
        public T Value { get; private set; }
    }
}
