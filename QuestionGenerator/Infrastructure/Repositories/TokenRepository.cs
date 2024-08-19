using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using QuestionGenerator.Models.TokenModel;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly QuestionGeneratorContext _context;

        public TokenRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<Token> AddAsync(Token token)
        {
            await _context.Tokens.AddAsync(token);
            return token;
        }

        public async Task<ICollection<Token>> GetAllAsync(Expression<Func<Token, bool>> exp)
        {
            var tokens = await _context.Tokens.Where(exp).ToListAsync();
            return tokens;
        }

        public async Task<Token> GetAsync(int id)
        {
            var token = await _context.Tokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            return token;
        }

        public async Task<Token> GetAsync(Expression<Func<Token, bool>> exp)
        {
            var tokenObject = await _context.Tokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(exp);
            return tokenObject;
        }

        public Token Remove(Token token)
        {
            token.IsDeleted = true;
            _context.Tokens.Update(token);
            return token;
        }

        public Token Update(Token token)
        {
            _context.Tokens.Update(token);
            return token;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var isValid = await _context.Tokens.AnyAsync(x => x.TokenValue.Equals(token));
            return isValid;
        }
    }
}
