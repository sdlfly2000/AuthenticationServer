using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Domain.User.Persistors;
using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;

namespace Infra.Database.Persistors
{
    [ServiceLocate(typeof(IUserPersistor))]
    public class UserPersistor : IUserPersistor
    {
        private readonly IdDbContext _dbContext;

        public UserPersistor(IdDbContext context)
        {
            _dbContext = context;
        }

        public async Task<DomainResult<UserReference>> Add(User user)
        {
            try
            {
                _ = await _dbContext.AddAsync(user);

                var result = await _dbContext.SaveChangesAsync();

                return new DomainResult<UserReference>()
                {
                    Id = (UserReference)user.Id,
                    Message = result > 0 ? "Success" : "Failure",
                    Success = result > 0
                };
            }
            catch (Exception ex)
            {
                return new DomainResult<UserReference>()
                {
                    Id = null,
                    Message = ex.Message,
                    Success = false
                };
            }
        }

        public async Task<DomainResult<UserReference>> Update(User user)
        {
            _dbContext.Update(user);

            var result = await _dbContext.SaveChangesAsync();

            return new DomainResult<UserReference>()
            {
                Id = (UserReference)user.Id,
                Message = result > 0 ? "Success" : "Failure",
                Success = result > 0
            };
        }
    }
}
