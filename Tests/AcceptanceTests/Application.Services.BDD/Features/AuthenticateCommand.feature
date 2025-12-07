Feature: Authenticate User

Authenticate User by UserName and PWD

@UserAcceptanceTest
Scenario: Authenticate User Successful
	Given an AuthenticateRequest with UserName "TestUser", Password "Password123" and UserAgent: "Chrome..."
	When Authenticate(Handle) is called
	Then the response should indicate successful authentication