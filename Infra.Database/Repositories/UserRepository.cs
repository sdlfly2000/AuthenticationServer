using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;
using Infra.Database.Entities;

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

        public async Task<DomainResult<UserReference>> Add(User user)
        {
            //var userEntity = User.Create(user);

            //await _context.AddAsync(userEntity);

            //var result = await _context.SaveChangesAsync();
            //return new DomainResult<UserReference>()
            //{
            //    Id = (UserReference)user.Id,
            //    Message = result > 0 ? "Success" : "Failure",
            //    Success = result > 0
            //};
            throw new NotImplementedException();
        }

        public User Find(UserReference reference)
        {
            throw new NotImplementedException();
        }

        public DomainResult<UserReference> Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
