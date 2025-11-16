using Application.Services.Events;
using Application.Services.User.Commands;
using Application.Services.User.ReqRes;
using Domain.User.Persistors;
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
    public class RegisterUserCommandHandlerTest
    {
        private const string UserName = "UserName";

        private Domain.User.Entities.User _user;
        private Mock<IUserPersistor> _userPersistorMock;
        private Mock<IBusService> _busServiceMock;
        private IServiceProvider _serviceProvider;

        private RegisterUserCommandHandler _registerUserCommandHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = Domain.User.Entities.User.Create(UserName);

            _busServiceMock = new Mock<IBusService>();
            _userPersistorMock = new Mock<IUserPersistor>();
            
            var serviceCollection= new ServiceCollection();
            serviceCollection.AddTransient<ILogger>((service) => Log.Logger);
            serviceCollection.AddTransient<IRequestTraceService>((service) => new RequestTraceService { TraceId = "TraceId" });
            _serviceProvider = serviceCollection.BuildServiceProvider();

            _userPersistorMock
                .Setup(p => p.Update(It.IsAny<Domain.User.Entities.User>()))
                .Returns(Task.FromResult(new DomainResult<UserReference>
                {
                    Message = string.Empty,
                    Success = true
                }));

            _registerUserCommandHandler = new RegisterUserCommandHandler(_userPersistorMock.Object, _busServiceMock.Object, _serviceProvider);
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public async Task Given_AddUserClaimRequest_When_handle_Then_AddUserClaimResponse_return()
        {
            // Arrange
            var claimType = ClaimTypes.Name;
            var claimValue = "ClaimValue";

            // Action
            var response = await _registerUserCommandHandler.Handle(new AddUserClaimRequest(_user.Id.Code, claimType, claimValue));

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, _user.Claims.Count);
            Assert.IsTrue(_user.Claims.Any(claim => claim.Name.Equals(claimType)));
            Assert.IsTrue(_user.Claims.Any(claim => claim.Value.Equals(claimValue)));
        }

    }
}
