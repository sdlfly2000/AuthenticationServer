using Application.Gateway.User;
using Application.Services.ReqRes;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.Authorization;
using Infra.Database;
using Reqnroll;
using System.Security.Claims;

namespace Application.Gateway.BDD.StepDefinitions
{
    [Binding]
    public class UserRoleAssignmentStepDefinitions(
            IUserGateway userGateway,
            IUserRepository userRepository,
            IdDbContext dbContext)
    {
        private readonly IdDbContext _dbContext = dbContext;
        private readonly IUserGateway _userGateway = userGateway;
        private readonly IUserRepository _userRepository = userRepository;

        private AssignRoleRequest? _assignRoleRequest;
        private AssignRoleResponse? _assignRoleResponse;

        private string? _userId;
        private string? _appName;
        private string? _roleName;
        private string _roleValue => $"{_appName}:{_roleName}";
        
        [Given("User with UserId: {string} with Claim AppName: {string}")]
        public async Task GivenUserWithUserIdWithClaimAppName(string userId, string appName)
        {
            _userId = userId;
            _appName = appName;

            var user = await _userRepository.Find((UserReference)userId, CancellationToken.None).ConfigureAwait(false);
            user.AddClaim(ClaimTypesEx.AppsAuthenticated, appName);
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        [Given("RoleName: {string}")]
        public void GivenRoleName(string roleName)
        {
            Assert.IsNotNull(_userId);
            Assert.IsNotNull(_appName);

            _roleName = roleName;

            _assignRoleRequest = new AssignRoleRequest(_userId, _appName, roleName);
        }

        [When("AssignRole")]
        public async Task WhenAssignRole()
        {
            Assert.IsNotNull(_assignRoleRequest);
            _assignRoleResponse =  await _userGateway.AssignRole(_assignRoleRequest, CancellationToken.None).ConfigureAwait(false);
        }

        [Then("The claim role {string} was assigned to User {string} successfully")]
        public async Task ThenTheClaimRoleWasAssignedToUserSuccessfully(string roleName, string userId)
        {
            Assert.IsNotNull(_assignRoleResponse);
            Assert.IsTrue(_assignRoleResponse.Success);

            var user = await _userRepository.Find((UserReference)userId, CancellationToken.None).ConfigureAwait(false);
            
            Assert.IsTrue(user.Claims.Any(c => c.Name.Equals(ClaimTypes.Role) && c.Value.Equals(_roleValue)));
        }

        [AfterScenario("@UserAssignRoleSuccessful")]
        public async Task CleanUp()
        {
            Assert.IsNotNull(_userId);
            Assert.IsNotNull(_appName);
            Assert.IsNotNull(_roleName);

            var user = await _userRepository.Find((UserReference)_userId, CancellationToken.None).ConfigureAwait(false);
            user.DeleteClaim(ClaimTypesEx.AppsAuthenticated, _appName);
            user.DeleteClaim(ClaimTypes.Role, _roleValue);
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
