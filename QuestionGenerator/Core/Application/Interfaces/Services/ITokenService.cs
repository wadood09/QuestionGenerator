using QuestionGenerator.Core.Domain.Enums;
using QuestionGenerator.Models;
using QuestionGenerator.Models.TokenModel;

namespace QuestionGenerator.Core.Application.Interfaces.Services
{
    public interface ITokenService
    {
        Task<BaseResponse<string>> GenerateToken(int userId, TokenType tokenType);
        Task<BaseResponse> ValidateToken(string token);
        Task<BaseResponse> DeactivateToken(string token);
    }
}
