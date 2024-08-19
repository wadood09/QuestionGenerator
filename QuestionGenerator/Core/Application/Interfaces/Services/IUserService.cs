using QuestionGenerator.Models.UserModel;
using QuestionGenerator.Models;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse> CreateUser(UserRequest request);
        Task<BaseResponse<UserResponse>> GetUser(int id);
        Task<BaseResponse<UserResponse>> GetUserByToken(string token);
        Task<BaseResponse<UserResponse>> GetUserByEmail(string email);
        Task<BaseResponse> UpdateUser(int id, UpdateUserRequest request);
        Task<BaseResponse> RemoveUser(int id);
        Task<BaseResponse> ResetPassword(string token, string newPassword);
    }
}
