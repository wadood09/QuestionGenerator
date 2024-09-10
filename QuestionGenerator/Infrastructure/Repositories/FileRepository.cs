using Microsoft.Extensions.Options;
using QuestionGenerator.Core.Application.Config;
using QuestionGenerator.Core.Application.Interfaces.Repositories;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly StorageConfig _config;
        private readonly HttpClient _httpClient;

        public FileRepository(IOptions<StorageConfig> config, HttpClient httpClient)
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<string?> UploadAsync(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            var allowedImageExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif", ".tif", ".tiff", ".bmp", ".webp", ".svg", ".ico", ".jfif" };
            var allowedDocumentExtensions = new HashSet<string> { ".pdf", ".doc", ".docx", ".ppt", ".pptx", ".txt" };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedImageExtensions.Contains(extension) && !allowedDocumentExtensions.Contains(extension))
            {
                throw new Exception("Unsupported file type.");
            }

            var newFileName = $"{Guid.NewGuid().ToString("N").Substring(0, 6)}" + $".{extension}";

            var basePath = _config.Path;
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            string savePath;
            if (allowedImageExtensions.Contains(extension))
            {
                var imagesPath = Path.Combine(basePath, "Images");
                if (!Directory.Exists(imagesPath))
                {
                    Directory.CreateDirectory(imagesPath);
                }
                savePath = Path.Combine(imagesPath, newFileName);
            }
            else
            {
                var documentsPath = Path.Combine(basePath, "Documents");
                if (!Directory.Exists(documentsPath))
                {
                    Directory.CreateDirectory(documentsPath);
                }
                savePath = Path.Combine(documentsPath, newFileName);
            }

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }


        public async Task<string?> SaveProfilePictureAsync(string imageUrl)
        {
            if (imageUrl == null)
                return null;

            var fileExtension = Path.GetExtension(imageUrl);
            var fileName = $"{Guid.NewGuid().ToString("N").Substring(0, 6)}.{fileExtension}";
            var folderPath = Path.Combine(_config.Path, "Images");
            var filePath = Path.Combine(folderPath, fileName);

            using (var response = await _httpClient.GetAsync(imageUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var imageBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(filePath, imageBytes);
                    return fileName;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
