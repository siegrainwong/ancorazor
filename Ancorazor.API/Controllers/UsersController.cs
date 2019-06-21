#region

using Ancorazor.API.Messages.Users;
using Ancorazor.Entity;
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
using Ancorazor.API.Controllers.Base;
using Ancorazor.Service;

#endregion

namespace Ancorazor.API.Controllers
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

            return Ok(new { user = new { user.Id, user.CreatedAt, user.RealName } });
        }

        [HttpPost("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        /// <summary>
        /// 在 SPA 初始化、凭据更换时刷新 XSRF Token
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpGet]
        [Route("XSRFToken")]
        public IActionResult GetXSRFToken()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
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
    }
}