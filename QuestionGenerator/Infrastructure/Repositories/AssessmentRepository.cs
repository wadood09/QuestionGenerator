using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class AssessmentRepository : IAssessmentRepository
    {
        private readonly QuestionGeneratorContext _context;

        public AssessmentRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<Assessment> AddAsync(Assessment assessment)
        {
            await _context.Assessments.AddAsync(assessment);
            return assessment;
        }

        public async Task<ICollection<Assessment>> GetAllAsync(Expression<Func<Assessment, bool>> exp)
        {
            var assessments = await _context.Assessments.Where(exp).ToListAsync();
            return assessments;
        }

        public async Task<ICollection<Assessment>> GetAllAsync()
        {
            var assessments = await _context.Assessments.ToListAsync();
            return assessments;
        }

        public async Task<Assessment> GetAsync(int id)
        {
            var assessment = await _context.Assessments
                .Include(x => x.Document)
                .Include(x => x.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(x => x.Id == id);
            return assessment;
        }

        public async Task<Assessment> GetAsync(Expression<Func<Assessment, bool>> exp)
        {
            var assessment = await _context.Assessments
                .Include(x => x.Document)
                .Include(x => x.Questions).ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(exp);
            return assessment;
        }

        public Assessment Remove(Assessment assessment)
        {
            assessment.IsDeleted = true;
            _context.Assessments.Update(assessment);
            return assessment;
        }

        public Assessment Update(Assessment assessment)
        {
            _context.Assessments.Update(assessment);
            return assessment;
        }
    }
}
