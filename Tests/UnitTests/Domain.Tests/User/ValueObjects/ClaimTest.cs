using Domain.User.Entities;
using Domain.User.ValueObjects;
using Infra.Core.Test;

namespace Domain.Tests.User.ValueObjects
{
    [TestClass]
    public class ClaimTest
    {
        private Claim _claim;

        [TestInitialize]
        public void TestInitialize()
        {
            _claim = new Claim(System.Security.Claims.ClaimTypes.Name,string.Empty);
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public void Given_Value_When_SetValue_Then_ValueAssinged()
        {
            // Arrange
            var value = "value";

            // Action
            _claim.SetValue(value);

            // Assert
            Assert.AreEqual(value, _claim.Value);
        }
    }
}
