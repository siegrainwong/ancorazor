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
                .AddCommandLine(args)
                .Build();
            Console.Clear();
            Console.WriteLine($"ProcessId: {Process.GetCurrentProcess().Id}");
            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                .UseStartup<Startup>().Build();
        }

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
    }
}