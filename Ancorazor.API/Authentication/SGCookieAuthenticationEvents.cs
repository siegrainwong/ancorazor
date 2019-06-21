using Ancorazor.Entity;
using Ancorazor.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ancorazor.API.Authentication
{
    public class SGCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        private readonly UserService _service;

        public SGCookieAuthenticationEvents(UserService service)
        {
            _service = service;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var url = context.Request.Path.ToString();
            if (url != "/" && !url.StartsWith("/api")) return;

            var claims = context.Principal.Claims;
            var loginName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            var authUpdatedTimeStr = claims.FirstOrDefault(x => x.Type == nameof(Users.AuthUpdatedAt)).Value;

            if (string.IsNullOrEmpty(authUpdatedTimeStr)) return;

            // sign out if user credential info(e.g. password) has changed
            var user = await _service.GetByLoginNameAsync(loginName);
            var truncated = user.AuthUpdatedAt.AddMilliseconds(-user.AuthUpdatedAt.Millisecond); // truncate millisecond for date comparison
            if (user == null ||
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
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public override async Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.Headers.Remove("Location");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await Task.CompletedTask.ConfigureAwait(false);
        }

        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            return base.RedirectToLogout(context);
        }
    }
}
