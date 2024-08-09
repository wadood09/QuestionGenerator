using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IQuestionResultRepository
    {
        Task<QuestionResult> AddAsync(QuestionResult result);
        Task<IEnumerable<QuestionResult>> AddRangeAsync(IEnumerable<QuestionResult> results);
        Task<QuestionResult> GetAsync(int id);
        Task<QuestionResult> GetAsync(Expression<Func<QuestionResult, bool>> exp);
        Task<ICollection<QuestionResult>> GetAllAsync();
        Task<ICollection<QuestionResult>> GetAllAsync(Expression<Func<QuestionResult, bool>> exp);
        QuestionResult Update(QuestionResult result);
        QuestionResult Remove(QuestionResult result);
    }
}
