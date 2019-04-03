#region

using Blog.API.Exceptions;
using Blog.API.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

#endregion

namespace Blog.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment env;
        private readonly ILogger<GlobalExceptionFilter> logger;

        public GlobalExceptionFilter(IHostingEnvironment env, ILogger<GlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            var exception = context.Exception;
            logger.LogError(
                new EventId(exception.HResult),
                exception,
                exception.Message);
            var errorCode = "0001";
            if (exception is APIException apiException) errorCode = apiException.ErrorCode;
            var errorResp = new ResponseMessage<object>
            {
                Message = exception.Message,
                ErrorCode = errorCode,
                Succeed = false
            };
            var result = new JsonResult(errorResp)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
            context.Result = result;
        }
    }
}