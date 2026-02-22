using Application.Gateway.User;
using Application.Gateway.User.Models;
using Application.Services.ReqRes;
using Domain.User.Repositories;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using System.Security.Claims;
using System.Text;

namespace Application.Gateway.BDD.StepDefinitions;

[Binding]
public class UserRegisterStepDefinitions(
    IUserGateway userGateway,
    IUserRepository userRepository,
    IdDbContext dbContext)
{
    private readonly IdDbContext _dbContext = dbContext;
    private readonly IUserGateway _userGateway = userGateway;
    private readonly IUserRepository _userRepository = userRepository;

    private RegisterUserRawRequest? _request;
    private RegisterUserResponse _response;

    private Guid _userId;
    private Domain.User.Entities.User _userRegistered;
    private string _userDisplayName;

    [BeforeScenario]
    public async Task Initial()
    {
    }

    [Given("Connect to Database {string}")]
    public void GivenConnectToDatabase(string databaseName)
    {
        Assert.AreEqual(databaseName, _dbContext.Database.GetDbConnection().Database);
    }

    [Given("Connect to MessageBus with host: {string}, Port: {int}, vhost: {string}")]
    public void GivenConnectToMessageBusWithHostPortVhost(string host, int port, string vhost)
    {
    }

    [Given("UserName: {string}, Password: {string}, DisplayName: {string}")]
    public void GivenUserNamePasswordDisplayName(string userName, string pwd, string displayName)
    {
        long unixSeconds = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        var pwdDateTime = string.Concat(pwd, "|", unixSeconds);
        var pwdEncrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(pwdDateTime));
        _request = new RegisterUserRawRequest(userName, pwdEncrypted, displayName);
    }

    [When("Register")]
    public async Task WhenRegister()
    {
        Assert.IsNotNull(_request);

        _response = await _userGateway.Register(_request, CancellationToken.None).ConfigureAwait(false);

        Assert.IsNotNull(_response);
    }

    [Then("User {string} is registered successfully")]
    public async Task ThenUserIsRegisteredSuccessfully(string userName)
    {
        Assert.IsTrue(_response.Success);

        var users = await _userRepository.GetAllUsers(CancellationToken.None);
        _userRegistered = users.Single(u => u.UserName == userName);

        Assert.IsNotNull(_userRegistered);
    }

    [Then("Claim NameIdentifier assigned to User {string} with its UserId")]
    public void ThenClaimNameIdentifierAssignedToUserWithItsUserId(string userName)
    {
        Assert.IsTrue(_userRegistered.Claims.Any(c => 
            c.Name.Equals(ClaimTypes.NameIdentifier) && 
            c.Value.Equals(_userRegistered.Id.Code)));
    }

    [Then("UserRegisteredEvent with DisplayName {string} has been delivered to MessageBus")]
    public async Task ThenUserRegisteredEventWithUserNameHasBeenDeliveredToMessageBus(string userName)
    {
        await Task.Delay(2);
        Assert.AreEqual(userName, _userDisplayName);
    }

    [Then("User {string} is registered unsuccessfully due to User already exists")]
    public void ThenUserIsRegisteredUnsuccessfullyDueToUserAlreadyExists(string userName)
    {
        Assert.IsFalse(_response.Success);
    }

    [AfterScenario("@UserAcceptanceTest","@UserRegistrationSuccessfull")]
    public async Task CleanUp()
    {
        if (_userRegistered is not null) 
        { 
            _dbContext.Set<Domain.User.Entities.User>().Remove(_userRegistered);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
