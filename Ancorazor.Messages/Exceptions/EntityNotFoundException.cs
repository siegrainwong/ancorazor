using Ancorazor.API.Common.Constants;
using Ancorazor.API.Messages.Exceptions.Attributes;
using Ancorazor.Entity.Base;
using System.Net;

namespace Ancorazor.API.Messages.Exceptions
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
