#region

using System;
using System.Runtime.Serialization;

#endregion

namespace Blog.API.Exceptions
{
    public class APIException : Exception
    {
        public string ErrorCode { get; set; }

        public APIException()
        {
            ErrorCode = "0001";
        }

        public APIException(string errorCode)
        {
            ErrorCode = errorCode;
        }

        public APIException(string errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

        public APIException(string errorCode, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ErrorCode = errorCode;
        }

        public APIException(string errorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}