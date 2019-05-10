using Blog.API.Controllers.Base;
using Blog.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController:SGControllerBase
    {
        private readonly SiteSettingService _settingService;

        public CommonController(SiteSettingService settingService, UserService userService) : base(userService)
        {
            _settingService = settingService;
        }

        [HttpGet("Setting")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSetting()
        {
            return Ok(await Task.FromResult(_settingService.GetSetting()));
        }
    }
}
