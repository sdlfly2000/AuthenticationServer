using Application.Services.ReqRes;
using Common.Core.CQRS.Request;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application.Services.BDD.StepDefinitions
{
    [Binding]
    public class AddUserClaimStepDefinitions
    {
        private const string UserId = "daca6f11-8ca8-4cd9-8197-2068ac993dec";
        private string _version;
        private AddUserClaimRequest _addUserClaimRequest;
        private AddUserClaimResponse _addUserClaimResponse;

        private readonly IdDbContext _dbContext;
        private readonly IRequestHandler<AddUserClaimRequest, AddUserClaimResponse> _handler;

        public AddUserClaimStepDefinitions(IdDbContext dbContext, IRequestHandler<AddUserClaimRequest, AddUserClaimResponse> handler)
        {
            _dbContext = dbContext;
            _handler = handler;
        }

        [AfterScenario("AddUserClaimSuccessful")]
        public async Task AfterScenario_AddUserClaimSuccessful()
        {
            // Add a Claim to user, with value version 2.0.1
            await _dbContext.Database.ExecuteSqlRawAsync($"DELETE FROM [IdentityTest].[dbo].[Claim] WHERE UserId='{UserId}' AND Name='{ClaimTypes.Version}' ").ConfigureAwait(false);
        }

        [Given("an AddUserClaimRequest with UserName {string} with UserId: {string}, ClaimType: {string}, ClaimValue: {string}")]
        public void GivenAnAddUserClaimRequestWithUserNameWithUserIdClaimTypeClaimValue(string userName, string userId, string version, string claimValue)
        {
            _version = claimValue;
            _addUserClaimRequest = new AddUserClaimRequest(userId, ClaimTypes.Version, claimValue);
        }

        [When("AddUserClaim\\(Handle) is called")]
        public async Task WhenAddUserClaimHandleIsCalled()
        {
            _addUserClaimResponse = await _handler.Handle(_addUserClaimRequest, CancellationToken.None).ConfigureAwait(false);

            Assert.IsTrue(_addUserClaimResponse.Success);
        }

        [Then("the response should indicate successful claim added")]
        public void ThenTheResponseShouldIndicateSuccessfulClaimAdded()
        {
            Assert.IsTrue(_dbContext.Set<Domain.User.ValueObjects.Claim>().Any(claim => claim.Name.Equals(ClaimTypes.Version) && claim.Value.Equals(_version)));
        }
    }
}
