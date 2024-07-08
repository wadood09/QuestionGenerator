using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Infrastructure.Context;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuestionGeneratorContext _context;

        public UnitOfWork(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
            var save = await _context.SaveChangesAsync();
            return save;
        }
    }
}
