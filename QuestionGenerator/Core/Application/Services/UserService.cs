using QuestionGenerator.Core.Application.Interfaces.Services;
using QuestionGenerator.Models;
using QuestionGenerator.Models.UserModel;

namespace QuestionGenerator.Core.Application.Services
{
    public class UserService : IUserService
    {
        public Task<BaseResponse> CreateUser(UserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<UserResponse>> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<UserResponse>> Login(LoginRequestModel model)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> RemoveUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse> UpdateUser(int id, UserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
