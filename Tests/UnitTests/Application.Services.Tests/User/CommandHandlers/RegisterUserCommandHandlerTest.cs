using Application.Services.ReqRes;
using Application.Services.User.Commands;
using Domain.User.Persistors;
using Domain.User.ValueObjects;
using Infra.Core.DomainBasics;
using Infra.Core.RequestTrace;
using Infra.Core.Test;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;

namespace Application.Services.Tests.User.CommandHandlers
{
    [TestClass]
    public class RegisterUserCommandHandlerTest
    {
        private const string UserName = "UserName";
        private const string Pwd = "Password";
        private const string DisplayName = "DisplayName";
        private const string UserCode = "A898ABED-F8EE-4E7F-8E5A-8D408A6E16F0";

        private Mock<IUserPersistor> _userPersistorMock;
        private IServiceProvider _serviceProvider;

        private RegisterUserCommandHandler _registerUserCommandHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            _userPersistorMock = new Mock<IUserPersistor>();
            
            var serviceCollection= new ServiceCollection();
            serviceCollection.AddTransient<ILogger>((service) => Log.Logger);
            serviceCollection.AddTransient<IRequestContext>((service) => new RequestContext { TraceId = "TraceId" });
            _serviceProvider = serviceCollection.BuildServiceProvider();

            _userPersistorMock
                .Setup(p => p.Add(It.IsAny<Domain.User.Entities.User>()))
                .Returns(Task.FromResult(new DomainResult<UserReference>
                {
                    Id = UserReference.Create(UserCode),
                    Message = string.Empty,
                    Success = true
                }));

            _registerUserCommandHandler = new RegisterUserCommandHandler(_userPersistorMock.Object, _serviceProvider);
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public async Task Given_RegisterUserRequest_When_handle_Then_RegisterUserResponse_return()
        {
            // Arrange
            var request = new RegisterUserRequest(UserName, Pwd, DisplayName);

            // Action
            var response = await _registerUserCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(string.Empty, response.Message);
        }
    }
}
