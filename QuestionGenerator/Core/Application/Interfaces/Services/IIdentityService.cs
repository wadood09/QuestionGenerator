using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IIdentityService
    {
        string GenerateAccessToken(string key, string issuer, string audience, UserResponse user);
        string GenerateRefreshToken();
        bool IsTokenValid(string key, string issuer, string audience, string token);
    }
}
