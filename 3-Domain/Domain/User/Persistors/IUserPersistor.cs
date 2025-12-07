using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;

namespace Domain.User.Persistors
{
    public interface IUserPersistor
    {
        Task<DomainResult<UserReference>> Add(Entities.User user);

        Task<DomainResult<UserReference>> Update(Entities.User user);
    }
}
