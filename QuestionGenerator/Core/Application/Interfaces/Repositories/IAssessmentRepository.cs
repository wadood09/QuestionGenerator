using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IAssessmentRepository
    {
        Task<Assessment> AddAsync(Assessment assessment);
        Task<Assessment> GetAsync(int id);
        Task<Assessment> GetAsync(Expression<Func<Assessment, bool>> exp);
        Task<int> GetAssessmentsTakenThisMonth(int userId);
        Task<ICollection<Assessment>> GetAllAsync(Expression<Func<Assessment, bool>> exp);
        Task<ICollection<Assessment>> GetAllAsync();
        Assessment Update(Assessment assessment);
        Assessment Remove(Assessment assessment);
    }
}
