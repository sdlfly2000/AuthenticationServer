using Application.Gateway.User;
using Application.Services.ReqRes;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.Authorization;
using Infra.Database;
using Reqnroll;

namespace Application.Gateway.BDD.StepDefinitions;

[Binding]
public class UserAppAssignmentStepDefinitions(
    IUserGateway userGateway,
    IUserRepository userRepository,
    IdDbContext dbContext)
{
    private readonly IdDbContext _dbContext = dbContext;
    private readonly IUserGateway _userGateway = userGateway;
    private readonly IUserRepository _userRepository = userRepository;

    private string? _userId;
    private string? _appName;
    private AssignAppRequest? _assignAppRequest;
    private AssignAppResponse? _assignAppResponse;

    [Given("User with UserId: {string}, AppName: {string}")]
    public void GivenUserWithUserIdAppName(string userId, string appName)
    {
        _userId = userId;
        _appName = appName;
        _assignAppRequest = new AssignAppRequest(userId, appName);
    }

    [When("AssignApp")]
    public async Task WhenAssignApp()
    {
        Assert.IsNotNull(_assignAppRequest);
        _assignAppResponse = await _userGateway.AssignApp(_assignAppRequest, CancellationToken.None).ConfigureAwait(false);
    }

    [Then("The claim AppsAuthenticated with value {string} was assigned to User {string} successfully")]
    public async Task ThenTheClaimAppsAuthenticatedWithValueWasAssignedToUserSuccessfully(string appName, string userId)
    {
        Assert.IsNotNull(_assignAppResponse);
        Assert.IsTrue(_assignAppResponse.Success);

        var user = await _userRepository.Find((UserReference)userId, CancellationToken.None).ConfigureAwait(false);
        Assert.IsTrue(user.Claims.Any(c => c.Name.Equals(ClaimTypesEx.AppsAuthenticated) && c.Value.Equals(appName)));            
    }

    [AfterScenario("@UserAssignAppSuccessful")]
    public async Task CleanUp()
    {
        Assert.IsNotNull(_userId);
        Assert.IsNotNull(_appName);

        var user = await _userRepository.Find((UserReference)_userId, CancellationToken.None).ConfigureAwait(false);
        user.DeleteClaim(ClaimTypesEx.AppsAuthenticated, _appName);
        _dbContext.Update(user);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
