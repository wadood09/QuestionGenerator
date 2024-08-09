using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;
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

        public async Task<AssesmentSubmission> AddAsync(AssesmentSubmission assesment)
        {
            await _context.AssessmentSubmissions.AddAsync(assesment);
            return assesment;
        }

        public async Task<ICollection<AssesmentSubmission>> GetAllAsync(Expression<Func<AssesmentSubmission, bool>> exp)
        {
            var assessments = await _context.AssessmentSubmissions
                .Include(x => x.Results).ThenInclude(y => y.Question)
                .Where(exp).ToListAsync();
            return assessments;
        }

        public async Task<ICollection<AssesmentSubmission>> GetAllAsync()
        {
            var assessments = await _context.AssessmentSubmissions
                .Include(x => x.Results).ThenInclude(y => y.Question)
                .ToListAsync();
            return assessments;
        }

        public async Task<AssesmentSubmission> GetAsync(int id)
        {
            var assessment = await _context.AssessmentSubmissions.FindAsync(id);
            return assessment;
        }

        public async Task<AssesmentSubmission> GetAsync(Expression<Func<AssesmentSubmission, bool>> exp)
        {
            var assessment = await _context.AssessmentSubmissions.FirstOrDefaultAsync(exp);
            return assessment;
        }

        public AssesmentSubmission Remove(AssesmentSubmission assesment)
        {
            _context.AssessmentSubmissions.Remove(assesment);
            return assesment;
        }

        public AssesmentSubmission Update(AssesmentSubmission assesment)
        {
            _context.AssessmentSubmissions.Update(assesment);
            return assesment;
        }
    }
}
