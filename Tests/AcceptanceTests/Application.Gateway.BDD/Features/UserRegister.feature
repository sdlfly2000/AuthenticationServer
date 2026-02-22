Feature: User Register

User Register

Background: 
	Given Connect to Database "IdentityTest"

@UserAcceptanceTest @UserRegistrationSuccessfull
Scenario: User Registration Successful
	Given UserName: "TestName", Password: "123456", DisplayName: "Test 1"
	When Register
	Then User "TestName" is registered successfully
	Then Claim NameIdentifier assigned to User "TestName" with its UserId

@UserAcceptanceTest @UserRegistrationFailure
@Ignore
Scenario: User Registration Failed due to Existing User
	Given UserName: "TestNameFailed", Password: "123456", DisplayName: "Test 2"
	When Register
	Then User "TestName" is registered unsuccessfully due to User already exists