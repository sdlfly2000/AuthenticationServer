using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Repositories
{
    [ServiceLocate(typeof(IUserRepository))]
    public class UserRepository : IUserRepository
    {
        private readonly IdDbContext _context;

        public UserRepository(IdDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindUserByUserNamePwd(string userName, string passwordHash)
        {
            return await _context.Set<User>()
                .Include(user => user.Claims)
                .SingleAsync(user => 
                    user.UserName.Equals(userName) && 
                    user.PasswordHash!.Equals(passwordHash));
        }

        public async Task<User> Find(UserReference reference)
        {
            return await _context.Set<User>()
                .Include(user => user.Claims)
                .SingleAsync(user => EF.Property<string>(user, "_id").Equals(reference.Code));
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Set<User>().ToListAsync();
        }
    }
}
