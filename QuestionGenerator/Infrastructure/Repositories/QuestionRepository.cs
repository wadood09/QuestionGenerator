using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly QuestionGeneratorContext _context;

        public QuestionRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<Question> AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            return question;
        }

        public async Task<ICollection<Question>> GetAllAsync(Expression<Func<Question, bool>> exp)
        {
            var questions = await _context.Questions.Where(exp).ToListAsync();
            return questions;
        }

        public async Task<ICollection<Question>> GetAllAsync()
        {
            var questions = await _context.Questions.ToListAsync();
            return questions;
        }

        public async Task<Question> GetAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            return question;
        }

        public async Task<Question> GetAsync(Expression<Func<Question, bool>> exp)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(exp);
            return question;
        }

        public Question Remove(Question question)
        {
            _context.Questions.Remove(question);
            return question;
        }

        public Question Update(Question question)
        {
            _context.Questions.Update(question);
            return question;
        }
    }
}
