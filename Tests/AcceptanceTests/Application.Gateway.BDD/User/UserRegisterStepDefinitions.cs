using Application.Gateway.User;
using Application.Gateway.User.Models;
using Application.Services.Events.Messages;
using Application.Services.User.ReqRes;
using Domain.User.Repositories;
using EasyNetQ;
using EasyNetQ.Topology;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Reqnroll;
using System.Text;

namespace Application.Gateway.BDD.User
{
    [Binding]
    public class UserRegisterStepDefinitions
    {
        private readonly IdDbContext _dbContext;
        private readonly IBus _messageBus;
        private readonly IUserGateway _userGateway;
        private readonly IUserRepository _userRepository;

        private RegisterUserRawRequest? _request;
        private RegisterUserResponse _response;

        private Guid _userId;
        private Domain.User.Entities.User _userRegistered;
        private string _userDisplayName;

        public UserRegisterStepDefinitions(
            IUserGateway userGateway,
            IUserRepository userRepository,
            IdDbContext dbContext, 
            IBus messageBus)
        { 
            _dbContext = dbContext;
            _messageBus = messageBus;
            _userGateway = userGateway;
            _userRepository = userRepository;
        }

        [BeforeScenario]
        public async Task Initial()
        {
            var bddQueueName = string.Concat(nameof(UserRegisterdEvent), "-", UserRegisterdEvent.RoutingKeyRegister);
            
            var bddQueue = await _messageBus.Advanced.QueueDeclareAsync(
                bddQueueName,
                (configuration) => configuration.AsDurable(true),
                CancellationToken.None)
                .ConfigureAwait(false);

            var exchange = await _messageBus.Advanced.ExchangeDeclareAsync(
                nameof(UserRegisterdEvent),
                (configuration) => configuration.AsDurable(true).WithType(ExchangeType.Topic))
                .ConfigureAwait(false);

            await _messageBus.Advanced.BindAsync(exchange, bddQueue, UserRegisterdEvent.RoutingKeyRegister)
                .ConfigureAwait(false);

            _messageBus.Advanced.Consume(
                bddQueue, 
                OnRegisteredMessageReceiver);
        }

        [Given("Connect to Database {string}")]
        public void GivenConnectToDatabase(string databaseName)
        {
            Assert.AreEqual(databaseName, _dbContext.Database.GetDbConnection().Database);
        }

        [Given("Connect to MessageBus with host: {string}, Port: {int}, vhost: {string}")]
        public void GivenConnectToMessageBusWithHostPortVhost(string host, int port, string vhost)
        {
            var msgBusConfigurations = _messageBus.Advanced.Container.Resolve<ConnectionConfiguration>();

            Assert.AreEqual(host, msgBusConfigurations.Hosts.First().Host);
            Assert.AreEqual(port, msgBusConfigurations.Hosts.First().Port);
            Assert.AreEqual(vhost, msgBusConfigurations.VirtualHost);
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
            Assert.AreEqual(true, _response.Success);

            var users = await _userRepository.GetAllUsers();
            _userRegistered = users.Single(u => u.UserName == userName);

            Assert.IsNotNull(_userRegistered);
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
            Assert.AreEqual(false, _response.Success);
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

        private async Task OnRegisteredMessageReceiver(ReadOnlyMemory<byte> body, MessageProperties properties, MessageReceivedInfo info)
        {
            var messageString = Encoding.UTF8.GetString(body.ToArray());
            var messagePayload = Newtonsoft.Json.JsonConvert.DeserializeObject<UserRegisterdEvent>(messageString);

            Assert.IsNotNull(messagePayload);

            _userId = messagePayload.UserId;
            _userDisplayName = messagePayload.DisplayName;
        }
    }
}
