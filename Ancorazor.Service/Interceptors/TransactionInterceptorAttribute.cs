using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Ancorazor.Entity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ancorazor.Service.Interceptors
{
    public class TransactionAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        private BlogContext Context { get; set; }
        [FromContainer]
        private ILogger<TransactionAttribute> Logger { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                Logger.LogInformation("begin transaction");
                try
                {
                    await next.Invoke(context);
                    transaction.Commit();
                    Logger.LogInformation("commit transaction");
                }
                catch
                {
                    transaction.Rollback();
                    Logger.LogInformation("transaction rollback");

                    throw;
                }
            }
        }
    }
}
