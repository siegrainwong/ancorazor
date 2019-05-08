#region

using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using AutoMapper;
using Blog.API.Authentication;
using Blog.API.AutoMapper;
using Blog.API.Common.Constants;
using Blog.API.Exceptions;
using Blog.API.Filters;
using Blog.EF.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Siegrain.Common;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Reflection;

#endregion

namespace Blog.API
{
    public class Startup
    {
        private const string _ServiceName = "Blog.API";

        public IConfiguration Configuration { get; }
        public ILogger<Startup> Logger { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;

            SetupLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            RegisterMapper(services);
            RegisterAppSettings(services);
            RegisterEntityFramework(services);
            RegisterMvc(services);
            RegisterRepository(services);
            RegisterService(services);
            RegisterSwagger(services);
            RegisterCors(services);
            RegisterAuthentication(services);
            RegisterSpa(services);
            ResigterProfiler(services);

            return services.ToServiceContainer().Build();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            ConfigureSwagger(app);
            //}

            app.UseCors();
            ConfigureAuthentication(app);
            app.UseHttpsRedirection();
            ConfigureMvc(app);
            ConfigureSpa(app, env);
        }

        private void SetupLogger()
        {
            var elasticUri = Configuration["ElasticConfiguration:Uri"];
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                    MinimumLogEventLevel = LogEventLevel.Information,
                    CustomFormatter = new CustomLogJsonFormmater()
                })
            .CreateLogger();
        }

        #region Services

        private void RegisterMapper(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(x =>
            {
                x.AddProfile<MappingProfile>();
                x.ValidateInlineMaps = false;   // ignore unmapped properties
            });

            var mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
        }

        private void RegisterAppSettings(IServiceCollection services)
        {
            services.Configure<SEOConfiguration>(x => Configuration.GetSection(nameof(SEOConfiguration)).Bind(x));
            services.Configure<DbConfiguration>(x => Configuration.GetSection(nameof(DbConfiguration)).Bind(x));
        }

        private void ResigterProfiler(IServiceCollection services)
        {
            // TODO: 还没弄好
            services.AddMiniProfiler(options =>
            {
                // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

                // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
                options.RouteBasePath = "/profiler";

                // (Optional)  To control which requests are profiled, use the Func<HttpRequest, bool> option:
                // (default is everything should be profiled)
                //options.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

                // (Optional) Profiles are stored under a user ID, function to get it:
                // (default is null, since above methods don't use it by default)
                //options.UserIdProvider = request => MyGetUserIdFunction(request);
            });
        }

        private void RegisterEntityFramework(IServiceCollection services)
        {
            /*
             * MARK: Parallel async method of ef core.
             * https://stackoverflow.com/questions/44063832/what-is-the-best-practice-in-ef-core-for-using-parallel-async-calls-with-an-inje
             */
            /*
             * MARK: EF 需要 Repository 模式吗？
             * 不需要，EF本身就带了仓储实现，再实现一层等于画蛇添足。
             * 即使需要封装也只需要针对 DbContext 和 DbSet 做扩展方法而已。
             */
            services.AddScoped<BlogContext, BlogContext>();
            services.AddDbContext<BlogContext>(options =>
                options.UseSqlServer(
                    Configuration[$"{nameof(DbConfiguration)}:{nameof(DbConfiguration.ConnectionString)}"]));
        }

        private void RegisterMvc(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<GlobalValidateModelFilter>();
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        private void RegisterCors(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddDefaultPolicy(policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            /**
             * MARK: Cookie based authentication
             * https://docs.microsoft.com/zh-cn/aspnet/core/security/authentication/cookie?view=aspnetcore-2.0&tabs=aspnetcore2x#persistent-cookies
             */
            services.AddSingleton<SGCookieAuthenticationEvents>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.EventsType = typeof(SGCookieAuthenticationEvents);
            });

            /**
             * MARK: Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core
             * https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-2.2
             */
            services.AddAntiforgery(options => { options.HeaderName = "X-XSRF-TOKEN"; });
        }

        private void RegisterRepository(IServiceCollection services)
        {
            services.AddSmartSql().AddRepositoryFromAssembly(o =>
            {
                o.AssemblyString = "Blog.Repository";
            });
        }

        private void RegisterService(IServiceCollection services)
        {
            var assembly = Assembly.Load("Blog.Service");
            var allTypes = assembly.GetTypes();
            foreach (var type in allTypes) services.AddScoped(type);
        }

        private void RegisterSpa(IServiceCollection services)
        {
            var section = Configuration.GetSection("Client");
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = $"{section["ClientPath"]}/dist";
            });
        }

        private void RegisterSwagger(IServiceCollection services)
        {
            // TODO: 换认证方式。。。
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = _ServiceName,
                    Version = "v1",
                    Description = "https://github.com/Seanwong933/siegrain.blog"
                });
                c.CustomSchemaIds(type => type.FullName);
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{_ServiceName}.xml");
                if (File.Exists(filePath)) c.IncludeXmlComments(filePath);

                var security = new Dictionary<string, IEnumerable<string>> { { _ServiceName, new string[] { } } };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition(_ServiceName, new ApiKeyScheme
                {
                    Description = "输入 Bearer {token}",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });
        }

        #endregion

        #region Configurations

        private void ConfigureAuthentication(IApplicationBuilder app)
        {
            app.Use(next => context =>
            {
                var contentType = context.Request.ContentType;
                if (!string.IsNullOrEmpty(contentType) &&
                    contentType.ToLower().Contains("application/x-www-form-urlencoded"))
                {
                    Logger.LogInformation(" Form submitting detected.");
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return context.Response.WriteAsync("Bad request.");
                }

                return next(context);
            });

            app.UseAuthentication();
        }

        private void ConfigureMvc(IApplicationBuilder app)
        {
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/api"),
                apiApp =>
                {
                    apiApp.UseMiniProfiler();
                    apiApp.UseMvc(routes =>
                    {
                        routes.MapRoute("default", "{controller}/{action=Index}/{id?}");
                    });
                });
        }

        /**
         * MARK: Angular 7 + .NET Core Server side rendering
         * https://github.com/joshberry/dotnetcore-angular-ssr
         */
        private void ConfigureSpa(IApplicationBuilder app, IHostingEnvironment env)
        {
            // now the static files will be served by new request URL
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // add route prefix for SSR
            app.Use((context, next) =>
            {
                // you can have different conditions to add different prefixes
                context.Request.Path = "/client" + context.Request.Path;
                return next.Invoke();
            });

            // MARK: 多 SPA 场景：https://stackoverflow.com/questions/48216929/how-to-configure-asp-net-core-server-routing-for-multiple-spas-hosted-with-spase
            var section = Configuration.GetSection("Client");
            // map spa to /client and remove the prefix
            app.Map("/client", client =>
            {
                client.UseSpa(spa =>
                {
                    spa.Options.SourcePath = section["ClientPath"];
                    spa.UseSpaPrerendering(options =>
                    {
                        options.BootModulePath = $"{spa.Options.SourcePath}/dist-server/main.js";
                        options.BootModuleBuilder = env.IsDevelopment()
                            ? new AngularCliBuilder("build:ssr")
                            : null;
                        options.ExcludeUrls = new[] { "/sockjs-node" };
                    });

                    if (env.IsDevelopment())
                    {
                        spa.UseAngularCliServer("start");
                    }
                });
            });
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger(c => { });
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", _ServiceName); });
        }

        #endregion

        #region Deprecated

        //private void RegisterAuthenticationForJwt(IServiceCollection services)
        //{
        //    /*
        //     MARK: JWT for session 从入门到放弃
        //     入门：
        //        - https://stackoverflow.com/questions/42036810/asp-net-core-jwt-mapping-role-claims-to-claimsidentity/50523668#50523668
        //        - Refresh token: https://auth0.com/blog/refresh-tokens-what-are-they-and-when-to-use-them/
        //        - How can I validate a JWT passed via cookies? https://stackoverflow.com/a/39386631
        //     攻击防治：
        //        - Where to store JWT in browser? How to protect against CSRF? https://stackoverflow.com/a/37396572
        //        - Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-2.2
        //     放弃：
        //        Stop using JWT for sessions
        //        - http://cryto.net/~joepie91/blog/2016/06/13/stop-using-jwt-for-sessions/
        //        - http://cryto.net/~joepie91/blog/2016/06/19/stop-using-jwt-for-sessions-part-2-why-your-solution-doesnt-work/

        //     总结：
        //        其本身并不适合拿来做 Session，Session 注定无法保证无状态，无法利用好 JWT 的优点，要强行用只能每次从认证服务器检查 refresh token 是否有效；
        //        很多现有的解决方案在你每次请求时检查 refresh token 后颁发一个新的 access token，然而旧的 access token 又在有效期内，多个 access_token 可以一起用听上去说实话挺2b的，所以为了让其“过期”，你又要维护一个 blacklist 或者 whitelist，再加上刷新方案自带的并发问题，说实话这套 JWT session 实践真的是一言难尽。

        //        等你把这套 access token、refresh token 全部实现下来，你会发现它还不如传统的 session 方案，而其中任何一步用了不恰当的实现方式，都会带来更多的安全漏洞。
        //     */
        //    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        //    var jwtSettings = Configuration.GetSection("Jwt");
        //    services
        //        .AddAuthentication(options =>
        //        {
        //            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //        })
        //        .AddJwtBearer(cfg =>
        //        {
        //            cfg.RequireHttpsMetadata = false;
        //            cfg.SaveToken = true;
        //            var rsa = RSACryptography.CreateRsaFromPrivateKey(Constants.RSAForToken.PrivateKey);
        //            cfg.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ClockSkew = TimeSpan.Zero, // remove delay of token when expire

        //                ValidIssuer = jwtSettings["JwtIssuer"],
        //                ValidAudience = jwtSettings["JwtIssuer"],
        //                IssuerSigningKey = new RsaSecurityKey(rsa),

        //                RequireExpirationTime = true,
        //                ValidateLifetime = true
        //            };
        //        });

        //    /**
        //     * MARK: 基于 JWT 预防 XSRF 和 XSS 攻击
        //     *  
        //     * - 将凭据（JWT）存放在 HttpOnly（无法被脚本访问）、SameSite=Strict（提交源跨域时不携带该Cookie）、Secure（仅HTTPS下携带该Cookie） 的 Cookie 中，而不是 LocalStorage 一类的地方。因为 Local Storage、Session Storage 会有 XSS 的风险，类似于 chrome extension 一类的东西可以随意读取这两类存储；而 Cookie 虽然有 XSRF 的风险，但可以通过双提交 Cookie 来预防，所以将凭证存放在 Cookie 依然是优先方案。
        //     * - 禁止 Form 表单提交，因为表单提交可以跨域。
        //     * - 使用 HTTPS
        //     * - 合理的过期机制
        //     * - 过滤用户输入来防止 XSS
        //     * - 在用户凭据变更后刷新 XSRF Token（刷新接口在 UserController -> GetXSRFToken）
        //     * - 禁止 HTTP TRACE 防止 XST 攻击（测试了一下好像默认就是禁止的）
        //     * - 由于 JWT Authentication 中间件是采用 Header Authorization 节进行验证，这里需要在Authentication 前加入一个中间件判断是否有 access token，有的话手动在 Header 中插入 Authorization 节以支持 JWT 验证。
        //     * 
        //     * - refs:
        //     *  Where to store JWT in browser? How to protect against CSRF? https://stackoverflow.com/a/37396572
        //     *  实现一个靠谱的Web认证：https://www.jianshu.com/p/805dc2a0f49e
        //     *  How can I validate a JWT passed via cookies? https://stackoverflow.com/a/39386631
        //     *  Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-2.2
        //     *  2 楼评论讨论了 refresh token 是否有意义，有不错的参考价值：https://auth0.com/blog/refresh-tokens-what-are-they-and-when-to-use-them/
        //     *  
        //     */
        //    services.AddAntiforgery(options => { options.HeaderName = "X-XSRF-TOKEN"; });
        //}
        #endregion
    }
}