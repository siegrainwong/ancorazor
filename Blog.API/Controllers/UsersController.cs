#region

using Blog.API.Messages;
using Blog.API.Messages.Users;
using Blog.Entity;
using Blog.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Siegrain.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUsersRepository _repository;
        private readonly IRoleRepository _roleRepository;

        public UsersController(IUsersRepository repository, IConfiguration configuration,
            IRoleRepository roleRepository)
        {
            _repository = repository;
            _configuration = configuration;
            _roleRepository = roleRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Token")]
        public async Task<IActionResult> GetToken([FromQuery] AuthUserParameter parameter)
        {
            var user = await _repository.GetByLoginNameAsync(parameter.LoginName);
            if (user == null || !SecurePasswordHasher.Verify(parameter.Password, user.Password)) return Forbid();

            var rehashTask = PasswordRehashAsync(user.Id, parameter.Password);
            var tokenTask = GenerateJwtTokenAsync(user); 
            await Task.WhenAll(rehashTask, tokenTask);
            
            // clear credentials
            user.LoginName = null;
            user.Password = null;

            return Ok(new ResponseMessage<object>
            {
                Data = new
                {
                    token = tokenTask.Result.Item1,
                    expires = tokenTask.Result.Item2,
                    user
                }
            });
        }

        [HttpGet]
        [Route("SignOut")]
        public IActionResult SignOut()
        {
            // Do nothing but refresh xsrf token;
            return Ok();
        }

        /// <summary>
        /// 重置密码
        ///
        /// Mark: Password encryption
        /// 1. 前端用 密码+用户名+域名 的MD5哈希值传递到后端
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
            var user = await _repository.GetByIdAsync(parameter.Id);
            if (user == null || !SecurePasswordHasher.Verify(parameter.Password, user.Password))
                return Forbid();

            return Ok(new ResponseMessage<object>
            {
                Succeed = await PasswordRehashAsync(parameter.Id, parameter.NewPassword)
            });
        }

        private async Task<bool> PasswordRehashAsync(int id, string password)
        {
            return await _repository.UpdateAsync(new
            {
                Id = id,
                Password = SecurePasswordHasher.Hash(password)
            }) > 0;
        }

        private async Task<Tuple<string, DateTime>> GenerateJwtTokenAsync(Users user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.LoginName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roles = await _roleRepository.GetRolesByUserAsync(user.Id);

            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)));

            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature, SecurityAlgorithms.Sha512Digest);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                jwtSettings["JwtIssuer"],
                jwtSettings["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            //var descriptor = new SecurityTokenDescriptor { Issuer = jwtSettings["JwtIssuer"], Audience = jwtSettings["JwtIssuer"], Subject = new ClaimsIdentity(claims), SigningCredentials = creds, Expires = expires };
            //var tokenhandler = new JwtSecurityTokenHandler();
            //var token = tokenhandler.CreateToken(descriptor);

            return new Tuple<string, DateTime>(new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}