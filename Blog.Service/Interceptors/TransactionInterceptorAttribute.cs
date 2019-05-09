using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Blog.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Blog.Service.Interceptors
{
    public class TransactionAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        private BlogContext Context { get; set; }
        [FromContainer]
        private ILogger<TransactionAttribute> logger { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var transaction = Context.Database.BeginTransaction();
            logger.LogInformation("begin transaction");
            try
            {
                await next.Invoke(context);
                transaction.Commit();
                logger.LogInformation("commit transaction");
            }
            catch
            {
                transaction.Rollback();
                logger.LogInformation("transaction rollback");
                throw;
            }
        }
    }
}
