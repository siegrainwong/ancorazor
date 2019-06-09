#region

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

#endregion

namespace Blog.API
{
    public class Program
    {
        public static IWebHost BuildWebHost(string[] args)
        {
            Console.WriteLine("2");
            var config = new ConfigurationBuilder()
                .Build();

            Console.WriteLine("3");
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://localhost:8088")
                .Build();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("1");
            BuildWebHost(args).Run();
        }
    }
}