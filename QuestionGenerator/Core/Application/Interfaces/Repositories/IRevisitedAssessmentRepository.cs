using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IRevisitedAssessmentRepository
    {
        Task<RevisitedAssesment> AddAsync(RevisitedAssesment assesment);
        Task<RevisitedAssesment> GetAsync(int id);
        Task<RevisitedAssesment> GetAsync(Expression<Func<RevisitedAssesment, bool>> exp);
        Task<ICollection<RevisitedAssesment>> GetAllAsync(Expression<Func<RevisitedAssesment, bool>> exp);
        Task<ICollection<RevisitedAssesment>> GetAllAsync();
        RevisitedAssesment Update(RevisitedAssesment assesment);
        RevisitedAssesment Remove(RevisitedAssesment assesment);
    }
}
