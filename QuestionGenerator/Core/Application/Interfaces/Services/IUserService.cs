using QuestionGenerator.Models.UserModel;
using QuestionGenerator.Models;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<BaseResponse> CreateUser(UserRequest request);

    }
}
