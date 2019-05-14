#region

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

#endregion

namespace Blog.API
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:8088/")
                .Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
    }
}