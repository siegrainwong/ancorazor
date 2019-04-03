#region

using Blog.API.Messages;
using Blog.API.Messages.Users;
using Blog.Entity;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private IConfiguration _configuration;
        private IUsersRepository _repository;
        private IRoleRepository _roleRepository;

        public UsersController(IUsersRepository repository, IConfiguration configuration,
            IRoleRepository roleRepository)
        {
            _repository = repository;
            _configuration = configuration;
            _roleRepository = roleRepository;
        }

        [HttpGet]
        [Route("Token")]
        public async Task<IActionResult> GetToken([FromQuery] AuthUserParameter parameter)
        {
            var user = await _repository.GetEntityAsync(parameter);
            if (user == null) return Forbid();

            var tokenInfo = await GenerateJwtTokenAsync(user);
            return Ok(new ResponseMessage<object>
            {
                Data = new
                {
                    token = tokenInfo.Item1,
                    expires = tokenInfo.Item2,
                    user
                }
            });
        }

        //[HttpGet]
        //[Route("RefreshToken")]
        //public async Task<IActionResult> RefreshToken([FromQuery] string token)
        //{
        //    if (token == null) return Forbid();
        //}

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
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(jwtSettings["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                jwtSettings["JwtIssuer"],
                jwtSettings["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new Tuple<string, DateTime>(new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}