using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Models.TokenModel;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        Task<Token> AddAsync(Token token);
        Task<Token> GetAsync(int id);
        Task<Token> GetAsync(Expression<Func<Token, bool>> exp);
        Task<ICollection<Token>> GetAllAsync(Expression<Func<Token, bool>> exp);
        Task<bool> ValidateTokenAsync(string token);
        Token Remove(Token token);
        Token Update(Token token);
    }
}
