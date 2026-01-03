using Domain.User.ValueObjects;

namespace Domain.User.Repositories
{
    public interface IUserRepository
    {
        Task<Entities.User> Find(UserReference reference);

        Task<List<Entities.User>> GetAllUsers();

        Task<Entities.User?> FindUserByUserNamePwd(string userName, string passwordHash);
    }
}
