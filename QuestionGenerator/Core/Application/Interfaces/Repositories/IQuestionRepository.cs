using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IQuestionRepository
    {
        Task<Question> AddAsync(Question question);
        Task<IEnumerable<Question>> AddRangeAsync(IEnumerable<Question> questions);
        Task<Question> GetAsync(int id);
        Task<Question> GetAsync(Expression<Func<Question, bool>> exp);
        Task<ICollection<Question>> GetAllAsync(Expression<Func<Question, bool>> exp);
        Task<ICollection<Question>> GetAllAsync();
        Question Update(Question question);
        Question Remove(Question question);
    }
}
