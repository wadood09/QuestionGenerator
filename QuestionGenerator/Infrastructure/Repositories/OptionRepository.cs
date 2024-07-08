using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class OptionRepository : IOptionRepository
    {
        private readonly QuestionGeneratorContext _context;

        public OptionRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<Option> AddAsync(Option option)
        {
            await _context.Options.AddAsync(option);
            return option;
        }

        public async Task<ICollection<Option>> GetAllAsync(Expression<Func<Option, bool>> exp)
        {
            var options = await _context.Options.Where(exp).ToListAsync();
            return options;
        }

        public async Task<ICollection<Option>> GetAllAsync()
        {
            var options = await _context.Options.ToListAsync();
            return options;
        }

        public async Task<Option> GetAsync(int id)
        {
            var option = await _context.Options.FindAsync(id);
            return option;
        }

        public async Task<Option> GetAsync(Expression<Func<Option, bool>> exp)
        {
            var option = await _context.Options.FirstOrDefaultAsync(exp);
            return option;
        }

        public Option Remove(Option option)
        {
            _context.Options.Remove(option);
            return option;
        }

        public Option Update(Option option)
        {
            _context.Options.Update(option);
            return option;
        }
    }
}
