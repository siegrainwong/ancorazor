using System;

namespace Blog.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// 过期时间，单位：分钟（该属性未实现）
        /// </summary>
        public int Expires { get; set; } = 30;
    }
}
