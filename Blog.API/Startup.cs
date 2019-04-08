#region

using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Blog.API.Common;
using Blog.API.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Siegrain.Common;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;

#endregion

namespace Blog.API
{
    public class Startup
    {
        private const string _ServiceName = "Blog.API";

        public IConfiguration Configuration { get; }
        public ILogger<Startup> Logger { get; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<GlobalValidateModelFilter>();
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
            RegisterRepository(services);
            RegisterService(services);
            RegisterSwagger(services);
            RegisterCors(services);
            RegisterAuthentication(services);
            RegisterSpa(services);

            var container = services.ToServiceContainer();
            return container.Build();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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

        #region Services

        private void RegisterCors(IServiceCollection services)
        {
            services.AddCors(c =>
            {
                c.AddDefaultPolicy(policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")

                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        private void RegisterAuthentication(IServiceCollection services)
        {
            // Mark: JWT Token https://stackoverflow.com/a/50523668

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var jwtSettings = Configuration.GetSection("Jwt");
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    var rsa = RSACryptography.CreateRsaFromPrivateKey(Constants.RSAForToken.PrivateKey);
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero, // remove delay of token when expire

                        ValidIssuer = jwtSettings["JwtIssuer"],
                        ValidAudience = jwtSettings["JwtIssuer"],
                        IssuerSigningKey = new RsaSecurityKey(rsa),

                        RequireExpirationTime = true,
                        ValidateLifetime = true
                    };
                });

            /**
             * MARK: 基于 JWT 预防 XSRF 和 XSS 攻击
             *  
             * - 将凭据（JWT）存放在 HttpOnly（无法被脚本访问）、SameSite=Strict（提交源跨域时不携带该Cookie）、Secure（仅HTTPS下携带该Cookie） 的 Cookie 中，而不是 LocalStorage 一类的地方。因为 Local Storage、Session Storage 会有 XSS 的风险，类似于 chrome extension 一类的东西可以随意读取这两类存储；而 Cookie 虽然有 XSRF 的风险，但可以通过双提交 Cookie 来预防，所以将凭证存放在 Cookie 依然是优先方案。
             * - 禁止 Form 表单提交，因为表单提交可以跨域。
             * - 使用 HTTPS
             * - 合理的过期机制
             * - 过滤用户输入来防止 XSS
             * - 在用户凭据变更后刷新 XSRF Token（刷新接口在 UserController -> GetXSRFToken）
             * - 禁止 HTTP TRACE 防止 XST 攻击（测试了一下好像默认就是禁止的）
             * - 由于默认的 JWT Authentication 中间件是通过 Header Authorize 节进行验证的，这里我添加一个 Filter 在 AuthorizeFilter 执行之前执行，判断是否有 JWT Cookie，有的话手动在 Header 中插入一个 Authorize 节以支持 JWT 进行验证。
             * 
             * - refs:
             *  Where to store JWT in browser? How to protect against CSRF? https://stackoverflow.com/a/37396572
             *  实现一个靠谱的Web认证：https://www.jianshu.com/p/805dc2a0f49e
             *  How can I validate a JWT passed via cookies? https://stackoverflow.com/a/39386631
             *  Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-2.2
             *  
             */
            services.AddAntiforgery(options => { options.HeaderName = "X-XSRF-TOKEN"; });
        }

        private void RegisterRepository(IServiceCollection services)
        {
            services.AddSmartSql()
                .AddRepositoryFromAssembly(o =>
                {
                    o.AssemblyString = "Blog.Repository";
                });
        }

        private void RegisterService(IServiceCollection services)
        {
            var assembly = Assembly.Load("Blog.Service");
            var allTypes = assembly.GetTypes();
            foreach (var type in allTypes) services.AddSingleton(type);
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = _ServiceName,
                    Version = "v1",
                    Description = "https://github.com/Seanwong933/siegrain.blog"
                });
                c.CustomSchemaIds(type => type.FullName);
                string filePath = Path.Combine(AppContext.BaseDirectory, $"{_ServiceName}.xml");
                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }

                Dictionary<string, IEnumerable<string>> security = new Dictionary<string, IEnumerable<string>> { { _ServiceName, new string[] { } } };
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
                // prohibited form submit
                var contentType = context.Request.ContentType;
                if (!string.IsNullOrEmpty(contentType) &&
                    contentType.ToLower().Contains("application/x-www-form-urlencoded"))
                {
                    Console.WriteLine(" Form submitting detected.");
                    context.Response.StatusCode = 400;
                    return context.Response.WriteAsync("Bad request.");
                }

                // fetch access_token from http only cookie
                // then append it into authorization header
                const string headerKey = "Authorization";
                if (context.Request.Cookies.TryGetValue("access_token", out string token))
                {
                    var headerToken = new StringValues($"Bearer {token}");
                    context.Request.Headers.Remove(headerKey);
                    context.Request.Headers.Append(headerKey, headerToken);
                    Logger.LogInformation($"{headerKey} token appended: {headerToken}");
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

            // Mark: 多 SPA 场景：https://stackoverflow.com/questions/48216929/how-to-configure-asp-net-core-server-routing-for-multiple-spas-hosted-with-spase
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
    }
}