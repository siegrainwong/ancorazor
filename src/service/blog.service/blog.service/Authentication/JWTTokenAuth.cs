using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Authentication
{
    public class JWTTokenAuth
    {
        private readonly RequestDelegate _next;
        public JWTTokenAuth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization")) return _next(httpContext);

            var tokenHeader = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var model = JWTHelper.SerializeJWT(tokenHeader);

            /*
             * Knowledge:
             * 理解 Claim、ClaimsIdentity、ClaimsPrincipal
             * 比如一个 Claim是身份证号，一个是名字，他们组合起来一张身份证，这个身份证就叫 ClaimsIdentity，可以证明自己的身份
             * 同样，一个人可以拥有多个自己的身份证明，于是多个 ClaimsIdentity 的持有者就是 ClaimsPrincipal
             */
            var claimList = new List<Claim>{ new Claim(ClaimTypes.Role, model.Role) };
            var identity = new ClaimsIdentity(claimList);
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            return _next(httpContext);
        }
    }
}
