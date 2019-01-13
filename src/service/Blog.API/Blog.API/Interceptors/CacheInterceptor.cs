using System;
using System.Linq;
using Blog.API.Caching;
using Castle.DynamicProxy;

namespace Blog.API.Interceptors
{
    public class CacheInterceptor : IInterceptor
    {
        private ICaching _cache;
        public CacheInterceptor(ICaching cache)
        {
            _cache = cache;
        }
        
        public void Intercept(IInvocation invocation)
        {
            //获取自定义缓存键
            var cacheKey = CustomCacheKey(invocation);
            //根据key获取相应的缓存值
            var cacheValue = _cache.Get(cacheKey);
            if (cacheValue != null)
            {
                //将当前获取到的缓存值，赋值给当前执行方法
                invocation.ReturnValue = cacheValue;
                return;
            }

            //去执行当前的方法
            invocation.Proceed();
            //存入缓存
            if (!string.IsNullOrWhiteSpace(cacheKey))
            {
                _cache.Set(cacheKey, invocation.ReturnValue);
            }
        }

        
        private static string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            var key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }

            return key.TrimEnd(':');
        }
        

        private static string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string) return arg.ToString();
            return arg is DateTime time ? time.ToString("yyyyMMddHHmmss") : "";
        }
    }
}
