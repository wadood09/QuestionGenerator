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
    }
}
