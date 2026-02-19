using Domain.User.ValueObjects;

namespace Domain.User.Repositories
{
    public interface IUserRepository
    {
        Task<Entities.User> Find(UserReference reference, CancellationToken token);

        Task<List<Entities.User>> GetAllUsers(CancellationToken token);

        Task<Entities.User> FindUserByUserNamePwd(string userName, string passwordHash);
    }
}
