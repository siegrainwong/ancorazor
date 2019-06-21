using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace Siegrain.Common
{
    public static class ImageProcessor
    {
        public static readonly string DirectorySeparator = Path.DirectorySeparatorChar.ToString();

        public static Task SaveWithThumbnailAsync(IFormFile file, int width, int height, string path, string fileName, string thumbPrefix = "thumb_")
        {
            var directoryPath = string.Concat(path.Replace("/", DirectorySeparator), DirectorySeparator);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            var fullPath = string.Concat(directoryPath, fileName);
            var thumbFullPath = string.Concat(directoryPath, thumbPrefix, fileName);
            using (var image = Image.Load(file.OpenReadStream()))
            {
                image.Save(fullPath);
                image.Mutate(x => x.Resize(width, height));
                image.Save(thumbFullPath);
            }
            return Task.CompletedTask;
        }
    }
}
