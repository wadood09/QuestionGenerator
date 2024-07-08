using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly QuestionGeneratorContext _context;

        public RoleRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<Role> AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            return role;
        }

        public async Task<ICollection<Role>> GetAllAsync(Expression<Func<Role, bool>> exp)
        {
            var roles = await _context.Roles.Where(exp).ToListAsync();
            return roles;
        }

        public async Task<ICollection<Role>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return roles;
        }

        public async Task<Role> GetAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            return role;
        }

        public async Task<Role> GetAsync(Expression<Func<Role, bool>> exp)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(exp);
            return role;
        }

        public Role Remove(Role role)
        {
            _context.Roles.Remove(role);
            return role;
        }

        public Role Update(Role role)
        {
            _context.Roles.Update(role);
            return role;
        }
    }
}
