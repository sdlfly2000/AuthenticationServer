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

        #region Service Method

        public static User Create(string userName)
        {
            return new User(userName);
        }

        public void AddClaim(string name, string value, string valueType)
        {
            var claim = new Claim(name, value, valueType);
            claim.AssignUser(_id);
            Claims.Add(claim);
        }

        public void UpdateClaim(string name, string value)
        {
            var claim = Claims.SingleOrDefault(claim => claim.Name.Equals(name));
            claim!.SetValue(value);
        }

        #endregion
    }
}
