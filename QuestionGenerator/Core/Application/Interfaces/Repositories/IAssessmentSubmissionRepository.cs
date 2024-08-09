using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IAssessmentSubmissionRepository
    {
        Task<AssesmentSubmission> AddAsync(AssesmentSubmission assesment);
        Task<AssesmentSubmission> GetAsync(int id);
        Task<AssesmentSubmission> GetAsync(Expression<Func<AssesmentSubmission, bool>> exp);
        Task<ICollection<AssesmentSubmission>> GetAllAsync(Expression<Func<AssesmentSubmission, bool>> exp);
        Task<ICollection<AssesmentSubmission>> GetAllAsync();
        AssesmentSubmission Update(AssesmentSubmission assesment);
        AssesmentSubmission Remove(AssesmentSubmission assesment);
    }
}
