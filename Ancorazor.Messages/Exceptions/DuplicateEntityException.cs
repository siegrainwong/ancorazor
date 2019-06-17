using Ancorazor.API.Common.Constants;
using Ancorazor.API.Messages.Exceptions.Attributes;
using Ancorazor.Entity.Base;
using System.Net;

namespace Ancorazor.API.Messages.Exceptions
{
    [APIExceptionCode(HttpStatusCode.Conflict, Constants.ErrorCode.DuplicateEntity)]
    public class DuplicateEntityException<T> : APIException where T : BaseEntity
    {
        public DuplicateEntityException(params string[] duplicateFields) : 
            base($"Duplicate fields({string.Join(" || ", duplicateFields)}) for entity {typeof(T).ToString()}.") { }
    }
}
