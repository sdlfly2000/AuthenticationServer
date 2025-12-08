using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using System.Diagnostics;

namespace Application.Services.BDD.StepDefinitions
{
    [Binding]
    public class AuthenticateUserStepDefinitions
    {
        private readonly IRequestHandler<AuthenticateRequest, AuthenticateResponse> _authenticateCommandHandler;

        private AuthenticateRequest? _request;
        private AuthenticateResponse? _response;

        public AuthenticateUserStepDefinitions(IRequestHandler<AuthenticateRequest, AuthenticateResponse> authenticateCommandHandler)
        {
            _authenticateCommandHandler = authenticateCommandHandler;
        }

        [Given("an AuthenticateRequest with UserName {string}, Password {string} and UserAgent: {string}")]
        public void GivenAnAuthenticateRequestWithUserNamePasswordAndUserAgent(string userName, string pwd, string userAgent)
        {
            _request = new AuthenticateRequest(userName, pwd, userAgent);
        }

        [When("Authenticate\\(Handle) is called")]
        public async Task WhenAuthenticateHandleIsCalled()
        {
            Debug.Assert(_request is not null);
            _response = await _authenticateCommandHandler.Handle(_request);
        }

        [Then("the response should indicate successful authentication")]
        public void ThenTheResponseShouldIndicateSuccessfulAuthentication()
        {
            Assert.IsNotNull(_response);
            Assert.IsTrue(_response.Success);
        }
    }
}
