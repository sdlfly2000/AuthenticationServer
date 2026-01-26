using Application.Services.ReqRes;
using Application.Services.User.CommandHandlers;
using Domain.User.Persistors;
using Domain.User.Repositories;
using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;
using Infra.Core.RequestTrace;
using Infra.Core.Test;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using System.Security.Claims;

namespace Application.Services.Tests.User.CommandHandlers
{
    [TestClass]
    public class AddUserClaimCommandHandlerTest
    {
        private const string UserName = "UserName";

        private Domain.User.Entities.User _user;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IUserPersistor> _userPersistorMock;
        private IServiceProvider _serviceProvider;

        private AddUserClaimCommandHandler _addUserClaimCommandHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = Domain.User.Entities.User.Create(UserName);

            _userPersistorMock = new Mock<IUserPersistor>();
            _userRepositoryMock= new Mock<IUserRepository>();
            
            var serviceCollection= new ServiceCollection();
            serviceCollection.AddTransient<ILogger>((service) => Log.Logger);
            serviceCollection.AddTransient<IRequestTraceService>((service) => new RequestTraceService { TraceId = "TraceId" });
            _serviceProvider = serviceCollection.BuildServiceProvider();

            _userRepositoryMock
                .Setup(repository => repository.Find(It.IsAny<UserReference>()))
                .Returns(Task.FromResult<Domain.User.Entities.User?>(_user));

            _userPersistorMock
                .Setup(p => p.Update(It.IsAny<Domain.User.Entities.User>()))
                .Returns(Task.FromResult(new DomainResult<UserReference>
                {
                    Message = string.Empty,
                    Success = true
                }));
                      
            _addUserClaimCommandHandler = new AddUserClaimCommandHandler(_userRepositoryMock.Object, _userPersistorMock.Object, _serviceProvider);
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public async Task Given_AddUserClaimRequest_When_handle_Then_AddUserClaimResponse_return()
        {
            // Arrange
            var claimType = ClaimTypes.Name;
            var claimValue = "ClaimValue";

            // Action
            var response = await _addUserClaimCommandHandler.Handle(new AddUserClaimRequest(_user.Id.Code, claimType, claimValue), CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, _user.Claims.Count);
            Assert.IsTrue(_user.Claims.Any(claim => claim.Name.Equals(claimType)));
            Assert.IsTrue(_user.Claims.Any(claim => claim.Value.Equals(claimValue)));
        }

    }
}
