using System;
using System.Linq;
using Blog.API.Caching;
using Blog.Common.Attributes;
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
            /*
             * Knowledge: Attributes from invocation.MethodInvocationTarget or invocation.Method
             * 前者拿的是 invocation.Proceed() 也就是下一个要执行的方法的 MethodInfo，后者拿的是接口的 MethodInfo，所以前者的不确定性要大一些
             * 因为前者拿到的 MethodInfo 有三个可能：
             * 1. 如果你有多个 Interceptor 的话，这里拿的可能是你的下一个 Interceptor
             * 2. 你接口的装饰器，如果有的话
             * 3. 你接口的最终实现（最后一个继承的实现）
             * 所以这里要取特性的话，最好在接口上放特性而不是在实现上
             * Ref: https://stackoverflow.com/questions/42493392/getting-attribute-value-on-member-interception
             */
            // 判断是否需要缓存
            if (invocation.Method.GetCustomAttributes(true).All(x => x.GetType() != typeof(CachingAttribute)))
            {
                invocation.Proceed();
                return;
            }

            var cacheKey = CustomCacheKey(invocation);
            var cacheValue = _cache.Get(cacheKey);
            // 拿到缓存则直接返回
            if (cacheValue != null)
            {
                invocation.ReturnValue = cacheValue;
                return;
            }

            invocation.Proceed();
            if (!string.IsNullOrWhiteSpace(cacheKey)) _cache.Set(cacheKey, invocation.ReturnValue);
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
