using QuestionGenerator.Models;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<BaseResponse<UserResponse>> LoginAsync(LoginRequest request);
        Task<BaseResponse<UserResponse>> VerifyGoogleTokenAsync(string idToken);
    }
}
