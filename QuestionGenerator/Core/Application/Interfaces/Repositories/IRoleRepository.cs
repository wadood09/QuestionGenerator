using QuestionGenerator.Core.Domain.Entities;
using System.Linq.Expressions;

namespace QuestionGenerator.Core.Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddAsync(Role role);
        Task<Role> GetAsync(int id);
        Task<Role> GetAsync(Expression<Func<Role, bool>> exp);
        Task<ICollection<Role>> GetAllAsync(Expression<Func<Role, bool>> exp);
        Task<ICollection<Role>> GetAllAsync();
        Role Update(Role role);
        Role Remove(Role role);
    }
}
