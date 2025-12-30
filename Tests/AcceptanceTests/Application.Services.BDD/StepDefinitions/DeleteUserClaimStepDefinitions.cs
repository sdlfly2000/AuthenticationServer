using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace Application.Services.BDD.StepDefinitions;

[Binding]
public class DeleteUserClaimStepDefinitions
{
    private const string UserId = "daca6f11-8ca8-4cd9-8197-2068ac993dec";
    private string ClaimId = Guid.NewGuid().ToString();
    private string _version;
    private DeleteUserClaimRequest _deleteUserClaimRequest;
    private DeleteUserClaimResponse _deleteUserClaimResponse;

    private readonly IdDbContext _dbContext;
    private readonly IRequestHandler<DeleteUserClaimRequest, DeleteUserClaimResponse> _handler;

    public DeleteUserClaimStepDefinitions(IdDbContext dbContext, IRequestHandler<DeleteUserClaimRequest, DeleteUserClaimResponse> handler)
    {
        _dbContext = dbContext;
        _handler = handler;
    }

    [BeforeScenario("DeleteUserClaimSuccessful")]
    public async Task BeforeScenario()
    {
        // Add a Claim to user, with value version 2.0.1
        await _dbContext.Database.ExecuteSqlRawAsync($"INSERT INTO [IdentityTest].[dbo].[Claim] (ClaimId, UserId, Name, Value, ValueType) VALUES ('{ClaimId}', '{UserId}', '{ClaimTypes.Version}', '2.0.1', '{ClaimValueTypes.String}')").ConfigureAwait(false);
    }

    [Given("an DeleteUserClaimRequest with UserName {string} with UserId: {string}, ClaimType: {string}, ClaimValue: {string}")]
    public void GivenAnDeleteUserClaimRequestWithUserNameWithUserIdClaimTypeClaimValue(string userName, string userId, string claimType, string claimValue)
    {
        _version = claimValue;
        _deleteUserClaimRequest = new DeleteUserClaimRequest(userId, ClaimTypes.Version, claimValue);
    }

    [When("DeleteUserClaim\\(Handle) is called")]
    public async Task WhenDeleteUserClaimHandleIsCalled()
    {
        _deleteUserClaimResponse = await _handler.Handle(_deleteUserClaimRequest).ConfigureAwait(false);

        Assert.IsTrue(_deleteUserClaimResponse.Success);
    }

    [Then("the response should indicate successful claim deletion")]
    public void ThenTheResponseShouldIndicateSuccessfulClaimDeletion()
    {
        Assert.IsFalse(_dbContext.Set<Domain.User.ValueObjects.Claim>().Any(claim => claim.Name.Equals(ClaimTypes.Version) && claim.Value.Equals(_version)));
    }
}
