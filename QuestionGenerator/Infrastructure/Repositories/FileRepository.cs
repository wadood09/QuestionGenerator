using Microsoft.Extensions.Options;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using static System.Net.Mime.MediaTypeNames;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly StorageConfig _config;

        public FileRepository(IOptions<StorageConfig> config)
        {
            _config = config.Value;
        }

        public async Task<string?> UploadAsync(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            var a = file.ContentType.Split('/');
            var newName = $"{a[0]}{Guid.NewGuid().ToString("N").Substring(0, 6)}.{a[1]}";

            var b = _config.Path;
            if (!Directory.Exists(b))
            {
                Directory.CreateDirectory(b);
            }
            string c;
            string[] imageExtensions = ["jpg", "jpeg", "png", "gif", "tif", "tiff", "bmp", "webp", "svg", "ico", "jfif"];
            if (imageExtensions.Contains(a[1]))
            {
                var images = Path.Combine(b, "Images");
                if(!File.Exists(images))
                {
                    File.Create(images);
                }
                c = Path.Combine(images, newName);
            }
            else
            {
                var document = Path.Combine(b, "Documents");
                if (!File.Exists(document))
                {
                    File.Create(document);
                }
                c = Path.Combine(document, newName);
            }

            using (var d = new FileStream(c, FileMode.Create))
            {
                await file.CopyToAsync(d);
            }

            return newName;
        }
    }
}
