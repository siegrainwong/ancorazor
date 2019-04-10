using Blog.Entity;
using Blog.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Linq;
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
            if (user == null ||
                string.IsNullOrEmpty(authUpdatedTimeStr) ||
                !DateTime.TryParse(authUpdatedTimeStr, out var authUpdatedAt) ||
                user.AuthUpdatedAt > authUpdatedAt)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
