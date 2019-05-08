#region

using Blog.API.Common.Constants;
using Blog.API.Messages;
using Blog.API.Messages.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using Siegrain.Common;
using Blog.API.Messages.Exceptions.Attributes;
using AspectCore.Extensions.Reflection;

#endregion

namespace Blog.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            var exception = context.Exception;
            logger.LogError(new EventId(exception.HResult), exception, exception.Message);

            var errorCode = Constants.ErrorCode.Default;
            var statusCode = HttpStatusCode.InternalServerError;
            if (exception is APIException apiException)
            {
                /*
                 MARK: Aspectcore reflection
                 https://github.com/dotnetcore/AspectCore-Framework/blob/master/docs/reflection-extensions.md
                 */
                var attribute = apiException.GetType().GetReflector().GetCustomAttribute<APIExceptionCodeAttribute>();
                if (attribute != null)
                {
                    errorCode = attribute.ErrorCode;
                    statusCode = attribute.StatusCode;
                }
            }

            var errorResp = new ResponseMessage<object>
            {
                Message = exception.Message,
                ErrorCode = errorCode,
                Succeed = false
            };

            context.Result = new JsonResult(errorResp) { StatusCode = (int)statusCode }; ;
        }
    }
}