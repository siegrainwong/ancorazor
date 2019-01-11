using Blog.Model.Authentication;
using Blog.Service.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获取JWT的重写方法，推荐这种，注意在文件夹OverWrite下
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sub">角色</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token2")]
        public string GetJWTStr(long id = 1, string sub = "Admin")
        {
            return JWTHelper.IssueJWT(new JWTTokenModel { Id = id, Role = sub });
        }
    }
}