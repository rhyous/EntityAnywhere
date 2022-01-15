using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Rhyous.EntityAnywhere.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class RestException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public RestException(string msg, HttpStatusCode statusCode) : base($"{(int)statusCode}:{statusCode} {msg}")
        {
            StatusCode = statusCode;
        }

        public RestException(HttpStatusCode statusCode) : base($"{(int)statusCode}:{statusCode}")
        {
            StatusCode = statusCode;
        }
    }
}