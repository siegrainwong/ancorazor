using Blog.API.Common.Constants;
using Blog.API.Messages.Exceptions.Attributes;
using Blog.Entity.Base;
using System.Net;

namespace Blog.API.Messages.Exceptions
{
    [APIExceptionCode(HttpStatusCode.Conflict, Constants.ErrorCode.DuplicateEntity)]
    public class DuplicateEntityException<T> : APIException where T : BaseEntity
    {
        public DuplicateEntityException(params string[] duplicateFields) : 
            base($"Duplicate fields({string.Join(" || ", duplicateFields)}) for entity {typeof(T).ToString()}.") { }
    }
}
