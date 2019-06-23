using Ancorazor.API.Common.Constants;
using Ancorazor.API.Controllers.Base;
using Ancorazor.API.Messages.Exceptions;
using Ancorazor.Entity;
using Ancorazor.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Siegrain.Common;
using Siegrain.Common.FileSystem;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ancorazor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : SGControllerBase
    {
        private readonly SiteSettingService _settingService;
        private readonly ImageStorageService _imageService;
        private readonly ImageProcessor _imageProcessor;
        private readonly ILogger<CommonController> _logger;

        public CommonController(SiteSettingService settingService, UserService userService, ImageStorageService imageService, ImageProcessor imageProcessor, ILogger<CommonController> logger) : base(userService)
        {
            _settingService = settingService;
            _imageService = imageService;
            _imageProcessor = imageProcessor;
            _logger = logger;
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
            var allowedFileExtensions = new[] { ".png", ".jpg", ".bmp" };

            if (string.IsNullOrEmpty(category)
                || preventedFileNameChars.Any(category.Contains)
                || file == null || file.Length < 1)
                throw new APIException("Empty file or invalid arguments.", HttpStatusCode.BadRequest);

            var fileExt = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
            if (!allowedFileExtensions.Contains(fileExt))
                throw new APIException($"{fileExt} is not a supported extension.", HttpStatusCode.BadRequest);

            var fileName = $"{Guid.NewGuid().ToString("N")}{fileExt}";
            var path = $"{Constants.UploadFilePath.Base}/{category}";

            _logger.LogInformation($"An image will upload to {path}");

            var entity = new ImageStorage
            {
                Category = category,
                Path = $"{path}/{fileName}",
                ThumbPath = $"{path}/thumb_{fileName}",
                Size = file.Length,
                Uploader = (await GetCurrentUser()).Id
            };

            await _imageProcessor.SaveWithThumbnailAsync(file, Constants.Article.ThumbWidth, Constants.Article.ThumbHeight, path, fileName);

            return Ok(new { Id = await _imageService.InsertAsync(entity) });
        }
    }
}
