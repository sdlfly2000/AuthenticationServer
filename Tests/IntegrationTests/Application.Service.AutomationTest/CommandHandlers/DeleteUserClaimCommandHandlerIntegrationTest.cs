using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using Common.Core.DependencyInjection;
using Domain.User.Entities;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.Test;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Diagnostics;
using System.Security.Claims;

namespace Application.Service.AutomationTest.CommandHandlers
{
    [TestClass]
    public class DeleteUserClaimCommandHandlerIntegrationTest
    {
        private static string UserId = "daca6f11-8ca8-4cd9-8197-2068ac993dec";    

        private static ServiceProvider? _serviceProvider;
        private static IdDbContext? _dbContext;
        private IRequestHandler<DeleteUserClaimRequest, DeleteUserClaimResponse> _handler;

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
                .RegisterDomain("AuthService", "Infra.Database", "Infra.Shared.Core", "Infra.Core", "Application.Services");

            // Add Serilog
            serviceCollection.AddSerilog(
                (configure) =>
                    configure.ReadFrom.Configuration(configuration));

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _dbContext = _serviceProvider.GetRequiredService<IdDbContext>();
        }

        [TestInitialize]
        public async Task TestInitialize()
        {
            Debug.Assert(_serviceProvider != null, "_serviceProvider is null.");

            _handler = _serviceProvider.GetRequiredService<IRequestHandler<DeleteUserClaimRequest, DeleteUserClaimResponse>>();
            var userPersistor = _serviceProvider.GetRequiredService<IUserPersistor>();
            var userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            var user = await userRepository.Find(UserReference.Create(UserId)).ConfigureAwait(false);

            Debug.Assert(user != null, $"user {UserId} is null in db IdentityTest.");

            user.AddClaim(ClaimTypes.Email, "TestEmail");
            await userPersistor.Update(user).ConfigureAwait(false);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (_dbContext is not null)
            {
                _dbContext.Dispose();
 
            }

            if( _serviceProvider is not null)
            {
                _serviceProvider?.Dispose();
            }
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.IntegrationTest))]
        public async Task Given_UserIdAndClaimTypeAndClaimValue_When_Handle_Then_ResponseReturn()
        {
            // Arrange
            var claimType = ClaimTypes.Email;
            var claimValue = "TestEmail";
            var request = new DeleteUserClaimRequest(UserId, claimType, claimValue);

            // Action
            var response = await _handler.Handle(request, CancellationToken.None).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);

            // Action
            var user = _dbContext?.Set<User>()
                .Include(user => user.Claims)
                .SingleOrDefault(user => EF.Property<string>(user, "_id").Equals(UserId));

            // Assert
            Assert.IsNotNull(user);
            Assert.IsFalse(user.Claims.Any(c => c.Name.Equals(claimType) && c.Value.Equals(claimValue)));
        }
    }
}