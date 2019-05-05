using Blog.API.Common.Constants;
using Blog.API.Messages.Exceptions.Attributes;
using System.Net;

namespace Blog.API.Messages.Exceptions
{
    [APIExceptionCode(HttpStatusCode.Conflict, Constants.ErrorCode.DuplicateEntity)]
    public class DuplicateEntityException : APIException
    {
        public DuplicateEntityException(string message): base(message) { }
    }
}
