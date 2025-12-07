using System;
using Reqnroll;

namespace Application.Services.BDD.StepDefinitions
{
    [Binding]
    public class AuthenticateUserStepDefinitions
    {
        [Given("an AuthenticateRequest with UserName {string}, Password {string} and UserAgent: {string}")]
        public void GivenAnAuthenticateRequestWithUserNamePasswordAndUserAgent(string testUser, string p1, string p2)
        {
            throw new PendingStepException();
        }

        [When("Authenticate\\(Handle) is called")]
        public void WhenAuthenticateHandleIsCalled()
        {
            throw new PendingStepException();
        }

        [Then("the response should indicate successful authentication")]
        public void ThenTheResponseShouldIndicateSuccessfulAuthentication()
        {
            throw new PendingStepException();
        }
    }
}
