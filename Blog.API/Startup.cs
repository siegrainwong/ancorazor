#region

using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using Blog.API.Filters;
using Microsoft.AspNetCore.Antiforgery;
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
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

namespace Blog.API
{
    public class Startup
    {
        private const string _ServiceName = "Blog.API";
        private const string _XSRFTokenName = "X-XSRF-TOKEN";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<GlobalValidateModelFilter>();
                // apply authorization by default
                //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            ConfigureSwagger(app);
            //}

            app.UseCors();
            app.UseAuthentication();
            ConfigureAuthentication(app, antiforgery);
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
            var jwtOptions = Configuration.GetSection("Jwt");
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
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtOptions["JwtIssuer"],
                        ValidAudience = jwtOptions["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            /**
             * MARK: 基于 JWT 认证预防 XSRF 和 XSS 攻击
             * 
             * https://stackoverflow.com/a/37396572
             * https://www.jianshu.com/p/805dc2a0f49e
             *  
             * 1. XSRF: X-XSRF-TOKEN 验证 https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-2.2
             * 2. 将 JWT 存放在 HttpOnly（无法被脚本访问）、SameSite=Strict（提交源跨域时不携带该Cookie）、Secure（仅HTTPS下携带该Cookie） 的 Cookie 中，而不是 LocalStorage 一类的地方。
             * 3. 禁止 Form 表单提交，因为表单提交可以跨域
             * 4. 使用 HTTPS
             * 5. 合理的过期过期机制
             * 6. 过滤用户输入来防止 XSS
             */
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = _XSRFTokenName;
                options.HeaderName = _XSRFTokenName;
                options.SuppressXFrameOptionsHeader = false;
            });
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
            Assembly assembly = Assembly.Load("Blog.Service");
            Type[] allTypes = assembly.GetTypes();
            foreach (Type type in allTypes) services.AddSingleton(type);
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

        private void ConfigureAuthentication(IApplicationBuilder app, IAntiforgery antiforgery)
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

                var path = context.Request.Path.Value;
                /*
                 这里有三个奇妙的坑
                 1. 在 UseSPA 时好像没有办法通过匹配首页来设置 XSRF Cookie，只能把它挂到 main.js 上。
                 2. 本来 Angular 在看到 XSRF Cookie 后应该自动设置到 Request Header上 的，结果写着写着突然就不行了。
                 3. BUG: 在用户凭据变更后老的 XSRF Token 失效，重新设置后依然提示 The provided antiforgery token was meant for a different claims-based user than the current user
                 */
                var tokenRefreshPaths = (new[] { "/main.js", "/api/Users/Token", "/api/Users/SignOut" }).AsQueryable();
                if (tokenRefreshPaths.Contains(path))
                {
                    // The request token can be sent as a JavaScript-readable cookie, 
                    // and Angular uses it by default.
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append(_XSRFTokenName, tokens.RequestToken,
                        new CookieOptions() { HttpOnly = false });
                    Console.WriteLine($" {_XSRFTokenName} written " + tokens.RequestToken);
                }

                return next(context);
            });
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