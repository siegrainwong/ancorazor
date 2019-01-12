using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog.Model;
using Blog.Model.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace Blog.API.Authentication
{
    internal class JWTHelper
    {
        internal const string SecretKey = "holyshitimsohandsome";
        public const string Issuer = "Blog.API";

        /// <summary>
        /// 颁发JWT字符串
        /// </summary>
        /// <param name="tokenModel"></param>
        /// <returns></returns>
        internal static string IssueJWT(JWTTokenModel tokenModel)
        {
            var claims = new[]
            {
                // 下边为Claim的默认配置
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                // 这个就是过期时间，目前是过期100秒，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss, Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, "wr"),
                // 这个Role是官方UseAuthentication要要验证的Role，我们就不用手动设置Role这个属性了
                new Claim(ClaimTypes.Role, tokenModel.Role),
            };

            //秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(Issuer, claims: claims, signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }
    }
}
