using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class AssessmentSubmissionRepository : IAssessmentSubmissionRepository
    {
        private readonly QuestionGeneratorContext _context;

        public AssessmentSubmissionRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<AssessmentSubmission> AddAsync(AssessmentSubmission assesment)
        {
            await _context.AssessmentSubmissions.AddAsync(assesment);
            return assesment;
        }

        public async Task<ICollection<AssessmentSubmission>> GetAllAsync(Expression<Func<AssessmentSubmission, bool>> exp)
        {
            var assessments = await _context.AssessmentSubmissions
                .Include(x => x.Results).ThenInclude(y => y.Question)
                .Where(exp).ToListAsync();
            return assessments;
        }

        public async Task<ICollection<AssessmentSubmission>> GetAllAsync()
        {
            var assessments = await _context.AssessmentSubmissions
                .Include(x => x.Results).ThenInclude(y => y.Question)
                .ToListAsync();
            return assessments;
        }

        public async Task<AssessmentSubmission> GetAsync(int id)
        {
            var assessment = await _context.AssessmentSubmissions
                .Include(x => x.Document)
                .Include(x => x.Assessment)
                .Include(x => x.Results)
                .ThenInclude(x => x.Question)
                .ThenInclude(x => x.Options)
                .FirstOrDefaultAsync(x => x.Id == id);
            return assessment;
        }

        public async Task<AssessmentSubmission> GetAsync(Expression<Func<AssessmentSubmission, bool>> exp)
        {
            var assessment = await _context.AssessmentSubmissions
                .Include(x => x.Document)
                .Include(x => x.Assessment)
                .Include(x => x.Results)
                .ThenInclude(x => x.Question)
                .ThenInclude(x => x.Options)
                .FirstOrDefaultAsync(exp);
            return assessment;
        }

        public AssessmentSubmission Remove(AssessmentSubmission assesment)
        {
            assesment.IsDeleted = true;
            _context.AssessmentSubmissions.Update(assesment);
            return assesment;
        }

        public AssessmentSubmission Update(AssessmentSubmission assesment)
        {
            _context.AssessmentSubmissions.Update(assesment);
            return assesment;
        }
    }
}
