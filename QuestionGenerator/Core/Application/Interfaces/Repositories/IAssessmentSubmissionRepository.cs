using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IAssessmentSubmissionRepository
    {
        Task<AssessmentSubmission> AddAsync(AssessmentSubmission assesment);
        Task<AssessmentSubmission> GetAsync(int id);
        Task<AssessmentSubmission> GetAsync(Expression<Func<AssessmentSubmission, bool>> exp);
        Task<ICollection<AssessmentSubmission>> GetAllAsync(Expression<Func<AssessmentSubmission, bool>> exp, int count = int.MaxValue);
        Task<ICollection<AssessmentSubmission>> GetAllAsync(int count = int.MaxValue);
        AssessmentSubmission Update(AssessmentSubmission assesment);
        AssessmentSubmission Remove(AssessmentSubmission assesment);
    }
}
