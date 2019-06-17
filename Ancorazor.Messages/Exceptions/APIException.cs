#region

using Ancorazor.API.Common.Constants;
using Ancorazor.API.Messages.Exceptions.Attributes;
using System;
using System.Net;
using System.Runtime.Serialization;

#endregion

namespace Ancorazor.API.Messages.Exceptions
{
    [APIExceptionCode(HttpStatusCode.InternalServerError, Constants.ErrorCode.Default)]
    public class APIException : Exception
    {
        public HttpStatusCode? StatusCode { get; set; }
        public APIException() { }
        public APIException(string message) : this(message, null) { }

        public APIException(string message, HttpStatusCode? statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}