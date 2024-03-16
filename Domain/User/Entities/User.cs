using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;

namespace Domain.User.Entities
{
    public class User : DomainEntity
    {
        public string UserName { get;}

        public string? PasswordHash { get;}

        public string? DisplayName { get;set;}

        public User(string userName) : base(new UserReference(Guid.NewGuid().ToString()))
        {
            UserName = userName;            
        }

        public static User Create(string userName)
        {
            return new User(userName);
        }

    }
}
