using Blog.API.Common.Constants;
using Blog.API.Messages.Exceptions.Attributes;
using Blog.Entity.Base;
using System.Net;

namespace Blog.API.Messages.Exceptions
{
    [APIExceptionCode(HttpStatusCode.NotFound, Constants.ErrorCode.DuplicateEntity)]
    public class EntityNotFoundException<T> : APIException where T : BaseEntity
    {
        public EntityNotFoundException()
            : base($"Entity of type {typeof(T).ToString()} not found.")
        {

        }
    }
}
