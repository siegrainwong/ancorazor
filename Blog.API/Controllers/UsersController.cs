#region

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blog.API.Messages;
using Blog.API.Messages.Users;
using Blog.Entity;
using Blog.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
            if (user != null)
            {
                user.Password = "";
                return Ok(new ResponseMessage<object>
                {
                    Data = new
                    {
                        token = await GenerateJwtTokenAsync(user), user
                    }
                });
            }
            else
                return Forbid();
        }

        private async Task<string> GenerateJwtTokenAsync(Users user)
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
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //[HttpPost]
        //public ResponseMessage<int> Insert([FromBody] Users users)
        //{
        //    return new ResponseMessage<int>
        //    {
        //        Data = UsersService.Insert(users)
        //    };
        //}

        //[HttpPost]
        //public ResponseMessage<int> DeleteById([FromBody] int id)
        //{
        //    return new ResponseMessage<int>
        //    {
        //        Data = UsersService.DeleteById(id)
        //    };
        //}

        //[HttpPost]
        //public ResponseMessage<int> Update([FromBody] Users users)
        //{
        //    return new ResponseMessage<int>
        //    {
        //        Data = UsersService.Update(users)
        //    };
        //}

        //[HttpPost]
        //public task ResponseMessage<Users> GetById([FromBody] int id)
        //{
        //    var users = UsersRepository.GetByIdAsync(id);
        //    return new ResponseMessage<Users>
        //    {
        //        Data = users
        //    };
        //}

        //[HttpPost]
        //public ResponseMessage<QueryResponse<Users>> Query([FromBody] QueryRequest reqMsg)
        //{
        //    var list = UsersRepository.Query(reqMsg);
        //    return new ResponseMessage<QueryResponse<Users>>
        //    {
        //        Data = new QueryResponse<Users>
        //        {
        //            List = list
        //        }
        //    };
        //}

        //[HttpPost]
        //public ResponseMessage<QueryByPageResponse<Users>> QueryByPage([FromBody] QueryByPageParameter reqMsg)
        //{
        //    var total = UsersRepository.GetRecord(reqMsg);
        //    var list = UsersRepository.QueryByPage(reqMsg);
        //    return new ResponseMessage<QueryByPageResponse<Users>>
        //    {
        //        Data = new QueryByPageResponse<Users>
        //        {
        //            Total = total,
        //            List = list
        //        }
        //    };
        //}
    }
}