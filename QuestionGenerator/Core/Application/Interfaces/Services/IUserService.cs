using QuestionGenerator.Models.UserModel;
using QuestionGenerator.Models;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse> CreateUser(UserRequest request);
        Task<BaseResponse<UserResponse>> GetUser(int id);
        Task<BaseResponse> UpdateUser(int id, UserRequest request);
        Task<BaseResponse> RemoveUser(int id);
        Task<BaseResponse<UserResponse>> Login(LoginRequestModel model);

    }
}
