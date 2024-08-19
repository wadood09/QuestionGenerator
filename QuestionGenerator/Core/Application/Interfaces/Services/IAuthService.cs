using QuestionGenerator.Models;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest model);
        Task<BaseResponse<UserResponse>> VerifyGoogleToken(string idToken);
        Task<BaseResponse> SendPasswordResetEmail(string email, string resetToken);
    }
}
