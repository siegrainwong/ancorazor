using Blog.API.Common.Constants;
using Blog.API.Controllers.Base;
using Blog.API.Messages.Exceptions;
using Blog.Entity;
using Blog.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Siegrain.Common;
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
        private readonly ImageStorageService _imageService;

        public CommonController(SiteSettingService settingService, UserService userService, ImageStorageService imageService) : base(userService)
        {
            _settingService = settingService;
            _imageService = imageService;
        }

        [HttpGet("Setting")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSetting()
        {
            return Ok(await Task.FromResult(_settingService.GetSetting()));
        }

        [HttpPost("Upload/{category}")]
        public async Task<IActionResult> UploadImage(IFormFile file, string category)
        {
            var preventedFileNameChars = new[] { '|', '\\', '?', '*', '<', '>', ':', '"', '\'' };

            if (string.IsNullOrEmpty(category)
                || preventedFileNameChars.Any(category.Contains)
                || file == null || file.Length < 1)
            {
                throw new APIException("Empty file or invalid arguments.", HttpStatusCode.BadRequest);
            }

            var fileExt = file.FileName.Substring(file.FileName.LastIndexOf("."));
            var fileName = $"{Guid.NewGuid().ToString("N")}{fileExt}";
            var path = $"{Constants.UploadFilePath.Base}/{category}";

            var entity = new ImageStorage
            {
                Category = category,
                Path = $"{path}/{fileName}",
                ThumbPath = $"{path}/thumb_{fileName}",
                Size = file.Length,
                Uploader = (await GetCurrentUser()).Id
            };

            await ImageProcessor.SaveWithThumbnailAsync(file, Constants.Article.ThumbWidth, Constants.Article.ThumbHeight, path, fileName);

            return Ok(new { Id = await _imageService.InsertAsync(entity) });
        }
    }
}
