using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ancorazor.API.Services
{
    public static class SpaPrerenderingServiceLocator
    {
        public static Action<HttpContext, IDictionary<string, object>> GetProcessor(IApplicationBuilder app)
        {
            return (httpContext, supplyData) =>
            {
                var service = app.ApplicationServices.CreateScope().ServiceProvider.GetService<ISpaPrerenderingService>();
                service.Process(httpContext, supplyData);
            };
        }
    }

    public interface ISpaPrerenderingService
    {
        void Process(HttpContext httpContext, IDictionary<string, object> supplyData);
    }

    public class SpaPrerenderingService : ISpaPrerenderingService
    {
        //private readonly ILogger _logger;
        //public SpaPrerenderingService(ILogger logger)
        //{
        //    _logger = logger;
        //}

        public void Process(HttpContext httpContext, IDictionary<string, object> supplyData)
        {
            supplyData["host"] = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.ToString()}";
            //_logger.LogInformation(httpContext.Request.Cookies);
        }
    }
}
