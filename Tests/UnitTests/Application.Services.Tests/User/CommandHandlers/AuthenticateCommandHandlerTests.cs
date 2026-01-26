using Application.Services.ReqRes;
using Application.Services.User.CommandHandlers;
using Common.Core.Authentication;
using Domain.User.Repositories;
using Infra.Core.Test;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Services.Tests.User.CommandHandlers
{
    [TestClass]
    public class AuthenticateCommandHandlerTest
    {
        private const string UserName = "UserName";
        private const string DisplayName = "DisplayName";
        private const string UserCode = "A898ABED-F8EE-4E7F-8E5A-8D408A6E16F0";

        private string Pwd = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(
                string.Concat("Password", "|", DateTime.Now.Ticks.ToString())));

        private Mock<IUserRepository> _userRepositoryMock;
        private IServiceProvider _serviceProvider;

        private AuthenticateCommandHandler _authenticateCommandHandler;

        [TestInitialize]
        public void TestInitialize()
        {
            Domain.User.Entities.User? user = new(UserName)
            {
                DisplayName = DisplayName
            };

            _userRepositoryMock = new Mock<IUserRepository>();

            _userRepositoryMock
                .Setup(p => p.FindUserByUserNamePwd(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(user));

            var jwtOptionsMock = new Mock<IOptions<JWTOptions>>();
            jwtOptionsMock
                .Setup(p => p.Value)
                .Returns(new JWTOptions
                {
                    ExpireSeconds = 86400, // 24 hr
                    Issuer = "AuthenticationService",
                    SigningKey = "fasdfad&9045dafz222#fadpio@0232121582"
                });

            _authenticateCommandHandler = new AuthenticateCommandHandler(_userRepositoryMock.Object, jwtOptionsMock.Object, _serviceProvider);    
        }

        [TestMethod, TestCategory(nameof(TestCategoryType.UnitTest))]
        public async Task Given_AuthenticateRequest_When_handle_Then_AuthenticateResponse_return()
        {
            // Arrange
            var request = new AuthenticateRequest(UserName, Pwd, DisplayName);

            // Action
            var response = await _authenticateCommandHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
        }
    }
}
