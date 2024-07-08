namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<string?> UploadAsync(IFormFile? file);
    }
}
