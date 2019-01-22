using Blog.Model.Authentication;
using Blog.API.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// 获取JWT的重写方法，推荐这种，注意在文件夹OverWrite下
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token")]
        public JsonResult GetToken(string username, string password)
        {
            string token;
            var suc = false;

            //这里就是用户登录以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了

            if (username == "admin" && password == "admin")
            {
                var model = new JWTTokenModel {Id = 1, Role = "Admin"};
                token = JWTHelper.IssueJWT(model);
                suc = true;
            }
            else
            {
                token = "login fail!!!";
            }

            return new JsonResult(new { succeed = suc, data = token });
        }
    }
}