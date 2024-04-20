using Application.Services.User.ReqRes;
using Common.Core.CQRS;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Infra.Core.Test;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Service.AutomationTest.CommandHandlers
{
    [TestClass]
    public class RegisterUserCommandHandlerAutomationTest
    {
        private const string UserName = "UserName";
        private static ServiceProvider? _serviceProvider;
        private static IdDbContext _dbContext;
        private IRequestHandler<RegisterUserRequest, RegisterUserResponse> _handler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var connectionString = "server=192.168.71.82;database=Identity;uid=sdlfly2000;password=sdl@1215;TrustServerCertificate=true";
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContextPool<IdDbContext>(
                options => options.UseSqlServer(connectionString)
            );
            serviceCollection
                .RegisterDomain("Infra.Database", "Infra.Shared.Core", "Infra.Core", "Application.Services")
                .RegisterNotifications("Application.Services");

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<IdDbContext>();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            var user = _dbContext?.Set<User>()
                .SingleOrDefault(user => user.UserName.Equals(UserName));

            _dbContext.Remove<User>(user!);

            _dbContext.SaveChanges();

            _dbContext.Dispose();
            _serviceProvider?.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = _serviceProvider!.GetRequiredService<IRequestHandler<RegisterUserRequest, RegisterUserResponse>>();
        }

        [TestMethod,TestCategory(nameof(TestCategoryType.AutomationTest))]
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