#region

using Ancorazor.API.Common.Constants;
using Ancorazor.API.Messages;
using Ancorazor.API.Messages.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using Siegrain.Common;
using Ancorazor.API.Messages.Exceptions.Attributes;
using AspectCore.Extensions.Reflection;
using AspectCore.DynamicProxy;

#endregion

namespace Ancorazor.API.Filters
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

            if (exception is AspectInvocationException aopException)
                exception = aopException.InnerException;

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

                if (apiException.StatusCode.HasValue)
                {
                    statusCode = apiException.StatusCode.Value;
                }
            }

            var errorResp = new ResponseMessage<object>
            {
                Message = exception.Message,
                ErrorCode = errorCode,
                Succeed = false
            };

            context.Result = new JsonResult(errorResp) { StatusCode = (int)statusCode };
        }
    }
}