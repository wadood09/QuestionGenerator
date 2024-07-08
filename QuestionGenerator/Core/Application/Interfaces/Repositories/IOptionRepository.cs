using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IOptionRepository
    {
        Task<Option> AddAsync(Option option);
        Task<Option> GetAsync(int id);
        Task<Option> GetAsync(Expression<Func<Option, bool>> exp);
        Task<ICollection<Option>> GetAllAsync(Expression<Func<Option, bool>> exp);
        Task<ICollection<Option>> GetAllAsync();
        Option Update(Option option);
        Option Remove(Option option);
    }
}
