using Microsoft.EntityFrameworkCore;
using QuestionGenerator.Core.Application.Interfaces.Repositories;
using QuestionGenerator.Core.Domain.Entities;
using QuestionGenerator.Infrastructure.Context;
using System.Linq.Expressions;

namespace QuestionGenerator.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly QuestionGeneratorContext _context;

        public UserRepository(QuestionGeneratorContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            var exists = await _context.Users.AnyAsync(x => x.Email == email);
            return exists;
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> exp)
        {
            var exists = await _context.Users.AnyAsync(exp);
            return exists;
        }

        public async Task<ICollection<User>> GetAllAsync(Expression<Func<User, bool>> exp)
        {
            var users = await _context.Users.Where(exp).ToListAsync();
            return users;
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<User> GetAsync(Expression<Func<User, bool>> exp)
        {
            var user = await _context.Users.Include(x => x.Role).FirstOrDefaultAsync(exp);
            return user;
        }

        public User Remove(User user)
        {
            user.IsDeleted = true;
            _context.Users.Update(user);
            return user;
        }

        public User Update(User user)
        {
            _context.Users.Update(user);
            return user;
        }
    }
}
