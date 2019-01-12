using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Blog.API
{
    public class Program
    {
        /*
         * 因为我们的web程序需要一个宿主, 所以 BuildWebHost 这个方法就创建了一个 WebHostBuilder. 而且我们还需要 Web Server.
           
           asp.net core 自带了两种http servers, 一个是 WebListener, 它只能用于windows系统, 另一个是kestrel, 它是跨平台的.
           
           kestrel是默认的web server, 就是通过UseKestrel()这个方法来启用的.
           
           但是我们开发的时候使用的是IIS Express, 调用UseIISIntegration()这个方法是启用IIS Express, 它作为Kestrel的Reverse Proxy server来用.
           
           如果在windows服务器上部署的话, 就应该使用IIS作为Kestrel的反向代理服务器来管理和代理请求.
           
           如果在linux上的话, 可以使用apache, nginx等等的作为kestrel的proxy server.
           
           当然也可以单独使用kestrel作为web 服务器, 但是使用iis作为reverse proxy还是有很多有优点的: 例如,IIS可以过滤请求, 管理证书, 程序崩溃时自动重启等.
           
           UseStartup<Startup>(), 这句话表示在程序启动的时候, 我们会调用Startup这个类.
           
           Build()完之后返回一个实现了IWebHost接口的实例(WebHostBuilder), 然后调用Run()就会运行Web程序, 并且阻止这个调用的线程, 直到程序关闭.
         */
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
