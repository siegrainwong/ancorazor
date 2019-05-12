using Blog.API.Common.Constants;
using Blog.API.Controllers.Base;
using Blog.API.Messages.Exceptions;
using Blog.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Siegrain.Common.FileSystem;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : SGControllerBase
    {
        private readonly SiteSettingService _settingService;
        private readonly IFileSystem _fileSystem;

        public CommonController(SiteSettingService settingService, UserService userService, IFileSystem fileSystem) : base(userService)
        {
            _settingService = settingService;
            _fileSystem = fileSystem;
        }

        [HttpGet("Setting")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSetting()
        {
            return Ok(await Task.FromResult(_settingService.GetSetting()));
        }

        [HttpPost("Upload/{category}")]
        public async Task<IActionResult> UploadFile(IFormFile file, string category)
        {
            var preventedFileNameChars = new[] { '|', '\\', '?', '*', '<', '>', ':', '"', '\'' };

            if (string.IsNullOrEmpty(category)
                || preventedFileNameChars.Any(category.Contains)
                || file == null || file.Length < 1)
            {
                throw new APIException("Empty file or invalid arguments.", HttpStatusCode.BadRequest);
            }

            category = category.Replace("/", _fileSystem.GetDirectorySeparatorChar());
            var fileExt = file.FileName.Substring(file.FileName.LastIndexOf("."));
            var fileName = $"{Guid.NewGuid().ToString("N")}{fileExt}";
            var subPath = string.Concat(category,
                _fileSystem.GetDirectorySeparatorChar(),
                fileName);

            var storageFile = await _fileSystem.CreateFileAsync(subPath);
            using (var outputStream = await storageFile.OpenWriteAsync())
            {
                await file.CopyToAsync(outputStream);
            }

            var fileUrl = $"{Constants.UploadFilePath.Base}/{category}/{fileName}";
            return Ok(new { PublicUrl = fileUrl, });
        }
    }
}
