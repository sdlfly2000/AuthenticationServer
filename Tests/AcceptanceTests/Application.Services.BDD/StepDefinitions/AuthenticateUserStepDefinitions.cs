using Application.Services.User.ReqRes;
using Common.Core.CQRS.Request;
using System.Diagnostics;

namespace Application.Services.BDD.StepDefinitions
{
    [Binding]
    public class AuthenticateUserStepDefinitions
    {
        private AuthenticateRequest? _authenticateRequest;
        private AuthenticateResponse? _authenticateResponse;

        private readonly IRequestHandler<AuthenticateRequest, AuthenticateResponse> _handler;

        public AuthenticateUserStepDefinitions(IRequestHandler<AuthenticateRequest, AuthenticateResponse> handler)
        {
            _handler = handler;
        }

        [Given("an AuthenticateRequest with UserName {string}, Password {string} and UserAgent: {string}")]
        public void GivenAnAuthenticateRequestWithUserNamePasswordAndUserAgent(string testUser, string pwd, string userAgent)
        {
            var pwdEncoded = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(
                string.Concat(pwd, "|", DateTime.Now.Ticks.ToString())));
            _authenticateRequest = new AuthenticateRequest(testUser, pwdEncoded, userAgent);
        }

        [When("Authenticate\\(Handle) is called")]
        public async Task WhenAuthenticateHandleIsCalled()
        {
            Debug.Assert(_authenticateRequest != null, "_authenticateRequest should not be null");
            _authenticateResponse = await _handler.Handle(_authenticateRequest, CancellationToken.None).ConfigureAwait(false);
        }

        [Then("the response should indicate successful authentication")]
        public void ThenTheResponseShouldIndicateSuccessfulAuthentication()
        {
            Assert.IsNotNull(_authenticateResponse);
            Assert.IsTrue(_authenticateResponse.Success);
        }
    }
}
