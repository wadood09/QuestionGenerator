using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<bool> ExistsAsync(string email);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> exp);
        Task<User> GetAsync(int id);
        Task<User> GetAsync(Expression<Func<User, bool>> exp);
        Task<ICollection<User>> GetAllAsync(Expression<Func<User, bool>> exp);
        Task<ICollection<User>> GetAllAsync();
        User Update(User user);
        User Remove(User user);
    }
}
