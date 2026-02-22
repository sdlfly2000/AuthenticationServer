using Common.Core.Domain;
using Domain.User.ValueObjects;

namespace Domain.User.Persistors
{
    public interface IUserPersistor
    {
        Task<DomainResult<UserReference>> Add(Entities.User user);

        Task<DomainResult<UserReference>> Update(Entities.User user);
    }
}
