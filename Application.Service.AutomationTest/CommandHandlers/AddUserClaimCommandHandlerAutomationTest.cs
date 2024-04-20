using Application.Services.User.ReqRes;
using Common.Core.CQRS;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.Test;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Application.Service.AutomationTest.CommandHandlers
{
    [TestClass]
    public class AddUserClaimCommandHandlerAutomationTest
    {
        private const string UserName = "UserName";
        private static string? UserNameId;    

        private static ServiceProvider? _serviceProvider;
        private static IdDbContext? _dbContext;
        private IRequestHandler<AddUserClaimRequest, AddUserClaimResponse> _handler;

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

            var userPersistor = _serviceProvider.GetRequiredService<IUserPersistor>();
            var domainResult = userPersistor.Add(User.Create(UserName)).Result;
            UserNameId = domainResult.Id.Code;

            _dbContext = _serviceProvider.GetRequiredService<IdDbContext>();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            var user = _dbContext!.Set<User>()
                .Include(user => user.Claims)
                .SingleOrDefault(user => EF.Property<string>(user, "_id").Equals(UserNameId));
            _dbContext.RemoveRange(user!.Claims);
            _dbContext.Remove(user);

            _dbContext.SaveChanges();

            _dbContext.Dispose();
            _serviceProvider?.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _handler = _serviceProvider!.GetRequiredService<IRequestHandler<AddUserClaimRequest, AddUserClaimResponse>>();
        }

        [TestMethod,TestCategory(nameof(TestCategoryType.AutomationTest))]
        public void Given_UserIdAndClaim_When_Handle_Then_ResponseReturn()
        {
            // Arrange
            var claimType = ClaimTypes.Email;
            var claimValue = "TestEmail";
            var request = new AddUserClaimRequest(UserNameId!, claimType, claimValue);

            // Action
            var response = _handler.Handle(request).Result;

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);

            // Action
            var user = _dbContext?.Set<User>()
                .Include(user => user.Claims)
                .SingleOrDefault(user => EF.Property<string>(user, "_id").Equals(UserNameId));

            // Assert
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Claims.Any(c => c.Name.Equals(claimType)));
            Assert.IsTrue(user.Claims.Any(c => c.Value.Equals(claimValue)));
        }
    }
}