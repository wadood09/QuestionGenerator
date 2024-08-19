using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly QuestionGeneratorContext _context;

        public DocumentRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<Document> AddAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            return document;
        }

        public async Task<bool> ExistsAsync(int userId, string title)
        {
            var exists = await _context.Documents.AnyAsync(x => title.Equals(x.Title, StringComparison.OrdinalIgnoreCase) && x.UserId == userId);
            return exists;
        }

        public async Task<ICollection<Document>> GetAllAsync(Expression<Func<Document, bool>> exp)
        {
            var documents = await _context.Documents.Where(exp).ToListAsync();
            return documents;
        }

        public async Task<ICollection<Document>> GetAllAsync()
        {
            var documents = await _context.Documents.ToListAsync();
            return documents;
        }

        public async Task<Document> GetAsync(int id)
        {
            var document = await _context.Documents.Include(x => x.Assessments).FirstOrDefaultAsync(x => x.Id == id);
            return document;
        }

        public async Task<Document> GetAsync(Expression<Func<Document, bool>> exp)
        {
            var document = await _context.Documents.Include(x => x.Assessments).FirstOrDefaultAsync(exp);
            return document;
        }

        public Document Remove(Document document)
        {
            document.IsDeleted = true;
            _context.Documents.Update(document);
            return document;
        }

        public Document Update(Document document)
        {
            _context.Documents.Update(document);
            return document;
        }
    }
}
