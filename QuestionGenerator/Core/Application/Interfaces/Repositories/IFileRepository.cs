namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<string?> UploadAsync(IFormFile? file);
        Task<string?> SaveProfilePictureAsync(string imageUrl);
    }
}
