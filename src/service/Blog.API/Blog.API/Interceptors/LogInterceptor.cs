using System;
using System.IO;
using System.Linq;
using Castle.DynamicProxy;

namespace Blog.API.Interceptors
{
    public class LogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // TODO: 这里以后换成SeriLog

            // 记录被拦截方法信息的日志信息
            var dataIntercept = $"{DateTime.Now:yyyyMMddHHmmss} " +
                                $"当前执行方法：{ invocation.Method.Name} " +
                                $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            // 在被拦截的方法执行完毕后 继续执行当前方法
            invocation.Proceed();

            dataIntercept += ($"被拦截方法执行完毕，返回结果：{invocation.ReturnValue}");

            var path = Directory.GetCurrentDirectory() + @"\Logs";
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var fileName = path + $@"\InterceptLog-{DateTime.Now:yyyyMMddHHmmss}.log";

            var sw = File.AppendText(fileName);
            sw.WriteLine(dataIntercept);
            sw.Close();
        }
    }
}
