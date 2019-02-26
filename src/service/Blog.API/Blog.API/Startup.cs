#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Blog.API.AutoMapper;
using Blog.API.Caching;
using Blog.API.Interceptors;
using Blog.Common.Configuration;
using Blog.Common.Redis;
using Blog.Model.Mapping;
using Blog.Model.ViewModel;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ProxyKit;
using Swashbuckle.AspNetCore.Swagger;

#endregion

namespace Blog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// DI 服务注册
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).
                AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .AddFluentValidation();
            var basePath = ApplicationEnvironment.ApplicationBasePath;

            #region SSR

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = $"{AppSettings.Get("AppSettings", "ClientPath")}/dist";
            });

            #endregion

            #region DI

            // for proxykit
            services.AddProxy();

            /*
             * Knowledge: Interceptor
             * 由于Interceptor内部原理是动态代理，会有较大的性能损耗，不建议在高并发系统上使用
             * 高并发系统应该使用IL静态注入式的AOP，比如这个 https://www.cnblogs.com/mushroom/p/3932698.html
             */
            services.AddScoped<ICaching, MemoryCaching>();
            services.AddScoped<IRedisCacheManager, RedisCacheManager>();

            // UrlHelper
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            // Model.Services
            var mappingContainer = new PropertyMappingContainer();
            mappingContainer.Register<ArticlePropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(mappingContainer);

            services.AddScoped<ITypeHelperService, TypeHelperService>();

            Mapper.Initialize(cfg => cfg.AddProfile<CustomProfile>());
            services.AddAutoMapper(typeof(Startup));

            #endregion

            #region CORS
            /*
             * Knowledge: 跨域配置
             * 将这种配置添加到Controller或Action上即可生效
             */
            services.AddCors(c =>
            {
                c.AddPolicy("LimitHosts", policy =>
                {
                    // TODO：这里如果request是http请求到https，会被CORS栏掉，暂不清楚原因
                    policy
                        .WithOrigins("http://localhost:4200", "http://localhost:5000", "https://localhost:5001")//支持多个域名端口
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v0.1.0",
                    Title = "blog.service API",
                    Description = "框架说明文档",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Blog.Core", Email = "siegrain@qq.com", Url = "" }
                });

                
                var xmlPath = Path.Combine(basePath, "Blog.API.xml");
                var xmlModelPath = Path.Combine(basePath, "Blog.Model.xml");    // 将其他类库的xml include进去，接口上用到这个实体时就会自动生成这个实体的传值范例
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(xmlModelPath, true);

                #region JWT

                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Blog.API", new string[] { } } };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("Blog.API", new ApiKeyScheme
                {
                    // 至于 Bearer 是 JWT 的约定，实际上你调用 app.UseAuthentication(); 之后可以不输。
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                #endregion
            });

            #endregion

            #region JWT

            // 现在用ID4，也可以随时切换回JWT

            //services.AddAuthorization(options =>
            //{
            //    // 添加权限组（策略）
            //    options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
            //    options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            //    options.AddPolicy("SystemAdmin", policy => policy.RequireRole("Admin", "System"));
            //    options.AddPolicy("SystemAdminOther", policy => policy.RequireRole("Admin", "System", "Other"));
            //})
            //.AddAuthentication(x =>
            //{
            //    // 
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = JWTHelper.Issuer,
            //        ValidAudience = "wr",
            //        ValidateLifetime = true,// 是否验证超时，当设置exp和nbf时有效，同时启用ClockSkew 
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWTHelper.SecretKey)),
            //        // 注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
            //        ClockSkew = TimeSpan.FromSeconds(30)
            //    };
            //});

            #endregion

            #region Database

            //BaseDBConfig.ConnectionString = Configuration.GetSection("AppSettings:SqlServerConnection").Value;

            #endregion

            #region AutoFac

            // 实例化 AutoFac 容器   
            var builder = new ContainerBuilder();

            /*
             * Knowledge: AutoFac
             * 这个地方是通过文件方式注入的，原有的程序集注入不知道为什么不行，导致找不到sqlsugar
             * 只能在API这里再用nuget装一次sqlsugar
             */
            
            builder.RegisterType<CacheInterceptor>(); // 拦截器在启用之前要先注册
            var assemblysServices = Assembly.LoadFile(Path.Combine(basePath, "Blog.Service.dll"));
            builder.RegisterAssemblyTypes(assemblysServices)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()  // 在 Blog.Service上 启用接口拦截
                .InterceptedBy(typeof(CacheInterceptor));
            var assemblysRepository = Assembly.LoadFile(Path.Combine(basePath, "Blog.Repository.dll"));
            builder.RegisterAssemblyTypes(assemblysRepository).AsImplementedInterfaces();

            // 将 services 填充到 Autofac 容器生成器中
            builder.Populate(services);

            // 使用已进行的组件登记创建新容器
            var ApplicationContainer = builder.Build();

            #endregion
            
            #region Identity Server

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = AppSettings.Get("AppSettings", "AuthorityHost");
                    options.ApiName = "restapi";
                });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("LimitHosts"));

                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            #endregion

            return new AutofacServiceProvider(ApplicationContainer);    // 用 Autofac 接管默认 DI 容器
        }

        /// <summary>
        /// 将服务解析出来用于请求管道中
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                });
                #endregion
            }
            else
            {
                app.UseHsts();
            }

            // redirection before pipeline
            app.UseHttpsRedirection();

            // api route
            app.MapWhen(context => context.Request.Path.StartsWithSegments("/api"),
                apiApp =>
                {
                    apiApp.UseMvc(routes =>
                    {
                        routes.MapRoute("default", "{controller}/{action=Index}/{id?}");
                    });
                });

            #region SSR

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

            // https://github.com/joshberry/dotnetcore-angular-ssr
            // 多 SPA 场景：https://stackoverflow.com/questions/48216929/how-to-configure-asp-net-core-server-routing-for-multiple-spas-hosted-with-spase
            // remove prefix and route
            // 只在个别路由开启 SSR
            app.Map("/client", client =>
            {
                // 只在个别路由开启 SSR
                client.MapWhen(context =>
                        !context.Request.Path.Equals("/add"),
                    partialRoute =>
                    {
                        partialRoute.UseSpa(spa =>
                        {
                            spa.Options.SourcePath = AppSettings.Get("AppSettings", "ClientPath");
                            spa.UseSpaPrerendering(options =>
                            {
                                options.BootModulePath = $"{spa.Options.SourcePath}/dist/server/main.js";
                                options.BootModuleBuilder = env.IsDevelopment()
                                    ? new AngularCliBuilder("build:ssr")
                                    : null;
                                options.ExcludeUrls = new[] {"/sockjs-node"};
                            });

                            if (env.IsDevelopment())
                            {
                                // 从这里启动会被强制指定一个随机port
                                spa.UseAngularCliServer("start");
                            }
                        });
                    });
                client.MapWhen(context =>
                    context.Request.Path.Equals("/add"),
                    partialRoute => {
                        partialRoute.RunProxy(context => context
                            .ForwardTo($"{AppSettings.Get("AppSettings", "ClientHost")}{context.Request.Path}")
                            .Send());
                });
            });

            #endregion

            app.UseCors("LimitHosts");
            // authentication
            app.UseAuthentication();
        }
    }
}
