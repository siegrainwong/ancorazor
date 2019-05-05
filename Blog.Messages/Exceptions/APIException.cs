#region

using Blog.API.Common.Constants;
using Blog.API.Messages.Exceptions.Attributes;
using System;
using System.Net;
using System.Runtime.Serialization;

#endregion

namespace Blog.API.Messages.Exceptions
{
    [APIExceptionCode(HttpStatusCode.InternalServerError, Constants.ErrorCode.Default)]
    public class APIException : Exception
    {
        public APIException() { }
        public APIException(string message) : base(message) { }
    }
}