using System;
using System.Net;

namespace Ancorazor.API.Messages.Exceptions.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class APIExceptionCodeAttribute : Attribute
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorCode { get; }
        public APIExceptionCodeAttribute(HttpStatusCode statusCode, string errorCode)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}
