using Blog.Entity;
using Blog.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.API.Authentication
{
    public class SGCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly IUsersRepository _repository;

        public SGCookieAuthenticationEvents(IUsersRepository repository)
        {
            _repository = repository;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var claims = context.Principal.Claims;
            var loginName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var authUpdatedTimeStr = claims.FirstOrDefault(x => x.Type == nameof(Users.AuthUpdatedAt)).Value;

            // sign out if user credential info(e.g. password) has changed
            var user = await _repository.GetByLoginNameAsync(loginName);
            var truncated = user.AuthUpdatedAt.AddMilliseconds(-user.AuthUpdatedAt.Millisecond); // truncate millisecond for date comparison
            if (user == null ||
                string.IsNullOrEmpty(authUpdatedTimeStr) ||
                !DateTime.TryParse(authUpdatedTimeStr, out var authUpdatedAt) ||
                truncated > authUpdatedAt)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        public override async Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.Headers.Remove("Location");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await Task.CompletedTask;
        }

        public override async Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.Headers.Remove("Location");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await Task.CompletedTask;
        }

        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            return base.RedirectToLogout(context);
        }
    }
}
