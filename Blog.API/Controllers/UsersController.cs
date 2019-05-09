#region

using Blog.API.Messages.Users;
using Blog.Entity;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Siegrain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.API.Controllers.Base;
using Blog.Service;

#endregion

namespace Blog.API.Controllers
{
    [ValidateAntiForgeryToken]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : SGControllerBase
    {
        private readonly IAntiforgery _antiforgery;
        private readonly UserService _service;
        private readonly RoleService _roleService;

        public UsersController(IAntiforgery antiforgery, UserService service, RoleService roleService) : base(service)
        {
            _antiforgery = antiforgery;
            _service = service;
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] AuthUserParameter parameter)
        {
            var user = await _service.GetByLoginNameAsync(parameter.LoginName);
            if (user == null || !SecurePasswordHasher.Verify(parameter.Password, user.Password))
                return Forbid();

            if (!await _service.PasswordRehashAsync(user, parameter.Password))
                return NotFound("Password rehash failed.");

            await SignIn(user);

            // clear credentials
            user.LoginName = null;
            user.Password = null;
            return Ok(new { user });
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        /// <summary>
        /// 在 SPA 初始化、凭据更换时刷新 XSRF Token
        ///
        /// MARK: XSRFToken refresh 为什么采取接口方案刷新？
        /// 
        /// TODO: 
        ///     这里好像可以在请求SignIn、SignOut接口的结尾再请求一波这个接口，直接把新的XSRFToken附上去？
        ///     登录后操作Header, Set-Cookie，把Cookie提取出来直接附加到Header里面去然后调用这个接口。
        ///     同理调用完注销接口后清掉Header中对应cookie即可。
        ///     那Cookie过期后怎么办？。。。在AuthenticationEvents里面判断吗？
        /// 
        /// 1. 因为不希望 SSR 时传递凭据，前端也需要做很多兼容处理。
        /// 2. XSRF Token 无法设置到首页上，估计也是 SSR 的锅，要设置只能把 Cookie 放在 main.js 上，非常怪异。
        /// 3. 本来是想尝试让请求过来时判断一下如果有凭据自动把 XSRFToken Cookie Append 到 Request.Cookie 上，但你 Append 不了 .AspNetCore.Antiforgery Cookie
        /// 还是迂回实现算了。
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet]
        [Route("XSRFToken")]
        public IActionResult GetXSRFToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });
            return Ok();
        }

        /// <summary>
        /// 重置密码
        ///
        /// MARK: Password hashing
        /// 1. 前端用 密码+用户名 的 PBKDF2 哈希值传递到后端
        /// 2. 后端检验后，用 CSPRNG 重新算盐，拼接密码哈希后用 PBKDF2 再哈希一次
        /// 3. 保存用户的新密码哈希
        /// 4. 每次用户登录后也要更新一次哈希值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordParameter parameter)
        {
            var user = await GetCurrentUser();
            if (user == null ||
                user.Id != parameter.Id ||
                !SecurePasswordHasher.Verify(parameter.Password, user.Password))
                return Forbid();

            var rehashTask = _service.PasswordRehashAsync(user, parameter.NewPassword, true);
            await Task.WhenAll(SignOut(), rehashTask);
            return Ok(succeed: rehashTask.Result);
        }

        #region Private Methods

        private async Task SignIn(Users user)
        {
            var roles = await _roleService.GetByUserAsync(user.Id);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.LoginName),
                new Claim(nameof(Users.AuthUpdatedAt), user.AuthUpdatedAt.ToString())
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)));
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });
        }

        #endregion

        #region Deprecated

        //[Obsolete("Jwt authentication is deprecated")]
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("Token")]
        //private async Task<IActionResult> GetToken([FromQuery] AuthUserParameter parameter)
        //{
        //    var user = await _repository.GetByLoginNameAsync(parameter.LoginName);
        //    if (user == null || !SecurePasswordHasher.Verify(parameter.Password, user.Password)) return Forbid();

        //    var rehashTask = PasswordRehashAsync(user.Id, parameter.Password);
        //    var tokenTask = GenerateJwtTokenAsync(user);
        //    await Task.WhenAll(rehashTask, tokenTask);

        //    // clear credentials
        //    user.LoginName = null;
        //    user.Password = null;

        //    Response.Cookies.Append("access_token", tokenTask.Result.token,
        //        new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

        //    return Ok(new { tokenTask.Result.expires, user });
        //}

        //[Obsolete("Jwt authentication is deprecated")]
        //private async Task<(string token, DateTime expires)> GenerateJwtTokenAsync(Users user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, user.LoginName),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        //    };

        //    var roles = await _roleRepository.GetRolesByUserAsync(user.Id);

        //    claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)));

        //    var rsa = RSACryptography.CreateRsaFromPrivateKey(Constants.RSAForToken.PrivateKey);
        //    var creds = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256Signature);

        //    var jwtSettings = _configuration.GetSection("Jwt");
        //    var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings["JwtExpireDays"]));
        //    var token = new JwtSecurityToken(
        //        jwtSettings["JwtIssuer"],
        //        jwtSettings["JwtIssuer"],
        //        claims,
        //        expires: expires,
        //        signingCredentials: creds
        //    );

        //    return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        //}
        #endregion
    }
}