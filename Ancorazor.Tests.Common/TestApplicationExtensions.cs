using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
//using Blog.API.Controllers;
//using Blog.API.Messages.Users;
using Ancorazor.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ancorazor.Tests.Common
{
    public static class TestApplicationExtensions
    {
        public static T CreateController<T>(this TestApplication app) where T : ControllerBase
        {
            var httpContext = app.GetService<IHttpContextFactory>().Create(new DefaultHttpContext().Features);
            httpContext.User = app.User;

            var routeData = new RouteData();
            routeData.Routers.Add(new FakeRoute(app.GetService<IInlineConstraintResolver>()));

            var actionContext = new ActionContext(
                httpContext,
                routeData,
                new ControllerActionDescriptor
                {
                    ControllerTypeInfo = typeof(T).GetTypeInfo()
                });

            var actionContextAccessor = app.GetService<IActionContextAccessor>();
            if (actionContextAccessor != null)
            {
                actionContextAccessor.ActionContext = actionContext;
            }
            return app.GetService<IControllerFactory>().CreateController(new ControllerContext(actionContext)) as T;
        }

        public static T GetService<T>(this TestApplication app) where T : class
        {
            return app.ApplicationServices.GetService<T>();
        }

        //public async static Task<Users> MockUser(this TestApplication app)
        //{
        //    var controller = CreateController<UsersController>(app);
        //    dynamic user = await controller.SignIn(new AuthUserParameter
        //    {
        //        LoginName = "admin",
        //        Password = "p5zm4O/fmIUy6/gRJMyBJQ=="   // 123456
        //    });
        //    return user.Data.User;
        //}

        public static Users NoUser(this TestApplication app)
        {
            app.ResetUser();
            return null;
        }

        //public static AdminUser MockAdminUser(this TestApplication app)
        //{
        //    var adminUserRepo = app.GetService<IRepository<AdminUser>>();
        //    var adminUserService = app.GetService<IAdminUserService>();

        //    var adminUser = new AdminUser
        //    {
        //        CreatedAtUtc = DateTime.UtcNow.AddDays(-1),
        //        Username = "AdminUser",
        //        HashedPassword = adminUserService.HashPassword("11111A")
        //    };
        //    adminUserRepo.Save(adminUser);

        //    var token = adminUserService.IssueJwtToken(adminUser);
        //    var options =  app.GetService<IOptionsMonitor<JwtBearerOptions>>().Get("Bearer");
        //    var identity = options.SecurityTokenValidators
        //                          .First()
        //                          .ValidateToken(token.TokenString, options.TokenValidationParameters, out _);
        //    app.User = identity;
        //    return adminUser;
        //}

        public static ModelStateDictionary ValidateModel(this TestApplication app, object model)
        {
            var validator = app.GetService<IObjectModelValidator>();
            var actionContext = new ActionContext();

            validator.Validate(actionContext, null, string.Empty, model);

            return actionContext.ModelState;
        }

        public static IEnumerable<StubLoggerProvider.LogItem> GetLogs(this TestApplication app)
        {
            var loggerProvider = app.ApplicationServices.GetRequiredService<ILoggerProvider>() as StubLoggerProvider;
            return loggerProvider?.LogItems;
        }

        public class FakeRoute : RouteBase
        {
            private List<VirtualPathContext> _urlGeneratingList = new List<VirtualPathContext>();
            public FakeRoute(IInlineConstraintResolver constraintResolver) : base("", "foo", constraintResolver, null, null, null)
            {
            }

            protected override Task OnRouteMatched(RouteContext context)
            {
                return Task.CompletedTask;
            }

            protected override VirtualPathData OnVirtualPathGenerated(VirtualPathContext context)
            {
                _urlGeneratingList.Add(context);
                return null;
            }

            public RouteValueDictionary GetGeneratedUrl => _urlGeneratingList.FirstOrDefault()?.Values;

            public List<RouteValueDictionary> GetGeneratedUrls
            {
                get { return _urlGeneratingList.Select(x => x.Values).ToList(); }
            }
        }


    }
}