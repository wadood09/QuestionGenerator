using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class RevisitedAssessmentRepository : IRevisitedAssessmentRepository
    {
        private readonly QuestionGeneratorContext _context;

        public RevisitedAssessmentRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<RevisitedAssesment> AddAsync(RevisitedAssesment assesment)
        {
            await _context.RevistedAssesments.AddAsync(assesment);
            return assesment;
        }

        public async Task<ICollection<RevisitedAssesment>> GetAllAsync(Expression<Func<RevisitedAssesment, bool>> exp)
        {
            var assessments = await _context.RevistedAssesments.Where(exp).ToListAsync();
            return assessments;
        }

        public async Task<ICollection<RevisitedAssesment>> GetAllAsync()
        {
            var assessments = await _context.RevistedAssesments.ToListAsync();
            return assessments;
        }

        public async Task<RevisitedAssesment> GetAsync(int id)
        {
            var assessment = await _context.RevistedAssesments.FindAsync(id);
            return assessment;
        }

        public async Task<RevisitedAssesment> GetAsync(Expression<Func<RevisitedAssesment, bool>> exp)
        {
            var assessment = await _context.RevistedAssesments.FirstOrDefaultAsync(exp);
            return assessment;
        }

        public RevisitedAssesment Remove(RevisitedAssesment assesment)
        {
            _context.RevistedAssesments.Remove(assesment);
            return assesment;
        }

        public RevisitedAssesment Update(RevisitedAssesment assesment)
        {
            _context.RevistedAssesments.Update(assesment);
            return assesment;
        }
    }
}
