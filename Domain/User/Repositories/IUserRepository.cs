using Domain.User.ValueObjects;

namespace Domain.User.Repositories
{
    public interface IUserRepository
    {
        UserReference Add(Entities.User user);
        UserReference Update(Entities.User user);

    }
}
