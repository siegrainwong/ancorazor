﻿using Blog.API.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers.Base
{
    public abstract class SGControllerBase : ControllerBase
    {
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
    }
}
