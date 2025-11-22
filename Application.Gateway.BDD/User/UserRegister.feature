Feature: User Register

User Register

Background: 
	Given Connect to Database "IdentityTest"
	And Connect to MessageBus with host: "rabbitmq.activator.com", Port: 5672, vhost: "BDD"

@UserAcceptanceTest @UserRegistrationSuccessfull
Scenario: User Registration Successful
	Given UserName: "TestName", Password: "123456", DisplayName: "Test 1"
	When Register
	Then User "TestName" is registered successfully
	And UserRegisteredEvent with DisplayName "Test 1" has been delivered to MessageBus

@UserAcceptanceTest @UserRegistrationFailure
Scenario: User Registration Failed due to Existing User
	Given UserName: "TestNameFailed", Password: "123456", DisplayName: "Test 2"
	When Register
	Then User "TestName" is registered unsuccessfully due to User already exists