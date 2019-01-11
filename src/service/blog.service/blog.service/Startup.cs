using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Service.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace Blog.Service
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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Blog.Service.xml");
                var xmlModelPath = Path.Combine(basePath, "Blog.Model.xml");    // 将其他类库的xml include进去，接口上用到这个实体时就会自动生成这个实体的传值范例
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(xmlModelPath, true);

                #region JWT

                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Blog.Service", new string[] { } } };
                c.AddSecurityRequirement(security);
                c.AddSecurityDefinition("Blog.Service", new ApiKeyScheme
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

            #region JWT 服务注册

            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            }).AddAuthorization(options =>
            {
                options.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                options.AddPolicy("AdminOrClient", policy => policy.RequireRole("Admin", "Client").Build());
            });

            //.AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,//是否验证Issuer
            //        ValidateAudience = true,//是否验证Audience 
            //        ValidateIssuerSigningKey = true,//是否验证IssuerSigningKey 
            //        ValidIssuer = "Blog.Core",
            //        ValidAudience = "wr",
            //        ValidateLifetime = true,//是否验证超时  当设置exp和nbf时有效 同时启用ClockSkew 
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtHelper.secretKey)),
            //        //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
            //        ClockSkew = TimeSpan.FromSeconds(30)

            //    };
            //});

            #endregion
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

            app.UseHttpsRedirection();

            // authentication
            app.UseMiddleware<JWTTokenAuth>();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
