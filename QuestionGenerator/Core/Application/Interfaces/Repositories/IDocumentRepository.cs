using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IDocumentRepository
    {
        Task<Document> AddAsync(Document document);
        Task<bool> ExistsAsync(string title);
        Task<Document> GetAsync(int id);
        Task<Document> GetAsync(Expression<Func<Document, bool>> exp);
        Task<ICollection<Document>> GetAllAsync(Expression<Func<Document, bool>> exp);
        Task<ICollection<Document>> GetAllAsync();
        Document Update(Document document);
        Document Remove(Document document);
    }
}
