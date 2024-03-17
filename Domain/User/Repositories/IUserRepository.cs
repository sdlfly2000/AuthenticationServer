using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;

namespace Domain.User.Repositories
{
    public interface IUserRepository
    {
        Task<DomainResult<UserReference>> Add(Entities.User user);
        DomainResult<UserReference> Update(Entities.User user);
        Entities.User Find(UserReference reference);
    }
}
