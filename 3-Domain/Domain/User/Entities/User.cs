using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;
using System.Text.Json.Serialization;

namespace Domain.User.Entities
{
    public class User(string userName) : DomainEntity<UserReference>
    {
        public UserReference Id => UserReference.Create(_id);

        public string UserName { get; set; } = userName;

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public string DisplayName { get;set;}

        public IList<Claim> Claims { get; private set; } = new List<Claim>();

        #region Database Usage

        private string _id { get; set; } = Guid.NewGuid().ToString();

        #endregion

        #region Service Methods

        public static User Create(string userName)
        {
            return new User(userName);
        }

        public void AddClaim(string name, string value, bool isFixed = false)
        {
            var claim = new Claim(name, value, isFixed);
            claim.AssignUser(_id);
            
            if(Claims.Any(c => c.Name.Equals(claim.Name) && c.Value.Equals(claim.Value)))
            {
                throw new InvalidOperationException($"Failure of adding Claim, Claim with Name: {name} already exists for user: {_id}.");
            }

            Claims.Add(claim);
        }

        public void UpdateClaim(string claimId, string value)
        {
            var claimReference = ClaimReference.Create(claimId);
            var claim = Claims.Single(claim => claim.Id.Equals(claimReference) && claim.IsFixed == false);
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
