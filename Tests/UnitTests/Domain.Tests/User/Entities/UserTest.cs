using Infra.Core.Test;
using System.Security.Claims;

namespace Domain.Tests.User.Entities
{
    [TestClass]
    public class UserTest
    {
        private const string UserName = "UserName";
        private const string ClaimType = ClaimTypes.Name;
        private const string ClaimValue = "ClaimValue";

        private Domain.User.Entities.User _user;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = Domain.User.Entities.User.Create(UserName);
            _user.AddClaim(ClaimType, ClaimValue);
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public void Given_UserName_When_Create_Then_User_Return()
        {
            // Arrange

            // Action
            var user = Domain.User.Entities.User.Create(UserName);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(UserName, user.UserName);
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public void Given_NameAndValue_When_AddClaim_Then_Claim_Added()
        {
            // Arrange
            var claimName = ClaimTypes.GivenName;
            var claimValue = "claimValue2";

            // Action
            _user.AddClaim(claimName, claimValue);

            // Assert
            Assert.AreEqual(2, _user.Claims.Count);
            Assert.IsTrue(_user.Claims.Any(claim => claim.Value.Equals(claimValue)));
            Assert.IsTrue(_user.Claims.Any(claim => claim.Name.Equals(claimName)));
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public void Given_NameAndValue_When_Update_Then_Claim_updated()
        {
            // Arrange
            var claimName = ClaimTypes.Name;
            var claimValue = "claimValueUpdated";

            // Action
            _user.UpdateClaim(claimName, claimValue);

            // Assert
            Assert.AreEqual(1, _user.Claims.Count);
            Assert.IsTrue(_user.Claims.Any(claim => claim.Value.Equals(claimValue)));
            Assert.IsTrue(_user.Claims.Any(claim => claim.Name.Equals(claimName)));
        }
    }
}
