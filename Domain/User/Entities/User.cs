using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;

namespace Domain.User.Entities
{
    public class User : DomainEntity
    {
        public string UserName { get; set; }

        public string? PasswordHash { get; set; }

        public string? DisplayName { get;set;}

        public IList<Claim> Claims { get; private set; }

        #region Database Usage

        private string _id { get; set; }

        #endregion

        public User(string userName) : base(new UserReference(Guid.NewGuid().ToString()))
        { 
            UserName = userName;
            Claims = new List<Claim>();

            _id = Id.Code;
        }

        public static User Create(string userName)
        {
            return new User(userName);
        }
    }
}
