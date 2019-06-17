using Ancorazor.API.Messages;
using Ancorazor.Entity;
using Ancorazor.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ancorazor.API.Controllers.Base
{
    public abstract class SGControllerBase : ControllerBase
    {
        private readonly UserService _service;
        public SGControllerBase(UserService service)
        {
            _service = service;
        }
        protected new OkObjectResult Ok() 
            => Ok<object>(data: default);

        protected virtual OkObjectResult Ok<T>(T data = default) 
            => Ok(succeed: true, data: data);

        protected virtual OkObjectResult Ok(bool succeed = true, object data = default, string message = null) 
            => Ok<object>(succeed, data, message);

        protected virtual OkObjectResult Ok<T>(bool succeed = true, T data = default, string message = null) =>
            base.Ok(new ResponseMessage<T>
            {
                Data = data,
                Succeed = succeed,
                Message = message
            });

        protected bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;

        protected async Task<Users> GetCurrentUser()
        {
            if (!IsAuthenticated) return null;

            var idStr = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!int.TryParse(idStr, out var id)) return null;

            return await _service.GetByIdAsync(id);
        }
    }
}
