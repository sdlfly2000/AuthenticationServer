using Domain.User.Entities;

namespace Infra.Database.Entities
{
    public class UserEntity : User
    {
        public EnumStatus Status { get; set; }

        public static UserEntity CreateFrom(User user)
        {
            return new UserEntity()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Status = EnumStatus.Active
            };
        }
    }
}
