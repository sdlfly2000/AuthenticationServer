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

        #region Service Methods

        public static User Create(string userName)
        {
            return new User(userName);
        }

        public void AddClaim(string name, string value, bool isFixed = false)
        {
            var claim = new Claim(name, value, isFixed);
            claim.AssignUser(_id);
            
            if(Claims.Any(c => c.Name.Equals(claim.Name)))
            {
                throw new InvalidOperationException($"Failure of adding Claim, Claim with Name: {name} already exists for user: {_id}.");
            }

            Claims.Add(claim);
        }

        public void UpdateClaim(string name, string value)
        {
            var claim = Claims.Single(claim => claim.Name.Equals(name) && claim.IsFixed == false);
            claim!.SetValue(value);
        }

        public bool DeleteClaim(string typeName, string value)
        {
            var claimToDelete = Claims
                .Single(c => c.Name.Equals(typeName) && c.Value.Equals(value) && c.IsFixed == false);

            return Claims.Remove(claimToDelete);
        } 

        #endregion
    }
}
