using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;

namespace Domain.User.Entities
{
    public class User : DomainEntity<UserReference>
    {
        public UserReference Id { get => UserReference.Create(_id); }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string DisplayName { get;set;}

        public IList<Claim> Claims { get; private set; }

        #region Database Usage

        private string _id { get; set; }

        #endregion

        public User(string userName)
        { 
            UserName = userName;
            Claims = new List<Claim>();
            _id = Guid.NewGuid().ToString();
        }

        public static User Create(string userName)
        {
            return new User(userName);
        }
    }
}
