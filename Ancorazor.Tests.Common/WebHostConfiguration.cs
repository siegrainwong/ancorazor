using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
namespace Ancorazor.Tests.Common
{
    public static class WebHostConfiguration
    {
        public static IWebHostBuilder Configure(IWebHostBuilder hostBuilder)
        {
            var config = new ConfigurationBuilder()
                .Build();

            return hostBuilder
                .UseConfiguration(config)
                .UseIISIntegration()
                .UseUrls("http://0.0.0.0:8088/");
        }
    }
}
