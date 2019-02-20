using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Blog.Common.Configuration
{
    /// <summary>
    /// appsettings.json 操作类
    /// </summary>
    public class AppSettings
    {
        static IConfiguration Configuration { get; }
        static AppSettings()
        {
            //ReloadOnChange = true 当appsettings.json被修改时重新加载
            Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
        }
        /// <summary>
        /// 封装要操作的字符
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static string Get(params string[] sections)
        {
            try
            {
                var val = sections.Aggregate(string.Empty, (current, t) => current + t + ":");
                return Configuration[val.TrimEnd(':')];
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
