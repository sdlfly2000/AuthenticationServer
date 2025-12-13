using Application.Services.User.ReqRes;
using Common.Core.CQRS;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Infra.Core.MessageQueue.RabbitMQ.Extentions;
using Infra.Core.Test;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Application.Service.AutomationTest.CommandHandlers
{
    [TestClass]
    public class RegisterUserCommandHandlerAutomationTest
    {
        private const string UserName = "UserNameForRegisteruserAutomationTest";
        private static ServiceProvider? _serviceProvider;
        private static IdDbContext _dbContext;
        private IRequestHandler<RegisterUserRequest, RegisterUserResponse> _handler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var configuration = new ConfigurationManager()
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();

            var serviceCollection = new ServiceCollection();

            // Test Database
            serviceCollection.AddDbContextPool<IdDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("IdentityDatabaseTest"))
            );

            // Register Services
            serviceCollection
                .RegisterDomain("Infra.Database", "Infra.Core.MessageQueue.RabbitMQ", "Infra.Shared.Core", "Infra.Core", "Application.Services")
                .RegisterNotifications("Application.Services");

            // Add Serilog
            serviceCollection.AddSerilog(
                (configure) =>
                    configure.ReadFrom.Configuration(configuration));

            // Add RabbitMQ support
            serviceCollection.AddRabbitMQBus(configuration);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<IdDbContext>();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            var user = _dbContext?.Set<User>()
                .SingleOrDefault(user => user.UserName.Equals(UserName));

            _dbContext.Remove<User>(user);

            _dbContext.SaveChanges();

            _dbContext.Dispose();
            _serviceProvider?.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = _serviceProvider!.GetRequiredService<IRequestHandler<RegisterUserRequest, RegisterUserResponse>>();
        }

        [TestMethod,TestCategory(nameof(TestCategoryType.IntegrationTest))]
        public async Task Given_UserNameAndPasswordAndDisplayName_When_Handle_Then_ResponseReturn()
        {
            // Arrange
            var Password = "Password";
            var DisplayName = "DisplayName";
            var request = new RegisterUserRequest(UserName, Password, DisplayName);

            // Action
            var response = await _handler.Handle(request);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);

            // Action
            var user = _dbContext?.Set<User>()
                .SingleOrDefault(user => user.UserName.Equals(UserName));

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(UserName, user.UserName);
            Assert.AreEqual(DisplayName, user.DisplayName);
        }
    }
}