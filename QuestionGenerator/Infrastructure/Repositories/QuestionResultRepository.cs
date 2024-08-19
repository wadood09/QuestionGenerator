using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class QuestionResultRepository : IQuestionResultRepository
    {
        private readonly QuestionGeneratorContext _context;

        public QuestionResultRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<QuestionResult> AddAsync(QuestionResult result)
        {
            await _context.Results.AddAsync(result);
            return result;
        }

        public async Task<IEnumerable<QuestionResult>> AddRangeAsync(IEnumerable<QuestionResult> results)
        {
            await _context.Results.AddRangeAsync(results);
            return results;
        }

        public async Task<ICollection<QuestionResult>> GetAllAsync()
        {
            var results = await _context.Results.ToListAsync();
            return results;
        }

        public async Task<ICollection<QuestionResult>> GetAllAsync(Expression<Func<QuestionResult, bool>> exp)
        {
            var results = await _context.Results.Where(exp).ToListAsync();
            return results;
        }

        public async Task<QuestionResult> GetAsync(int id)
        {
            var result = await _context.Results.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<QuestionResult> GetAsync(Expression<Func<QuestionResult, bool>> exp)
        {
            var result = await _context.Results.FirstOrDefaultAsync(exp);
            return result;
        }

        public QuestionResult Remove(QuestionResult result)
        {
            result.IsDeleted = true;
            _context.Results.Update(result);
            return result;
        }

        public QuestionResult Update(QuestionResult result)
        {
            _context.Results.Update(result);
            return result;
        }
    }
}
