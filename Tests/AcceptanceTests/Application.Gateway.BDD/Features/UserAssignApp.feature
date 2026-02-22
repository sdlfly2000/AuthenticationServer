Feature: User App Assignment

User App Assignment

Background: 
	Given Connect to Database "IdentityTest"

@UserAcceptanceTest @UserAssignAppSuccessful
Scenario: User App Assignment Successful
	Given User with UserId: "daca6f11-8ca8-4cd9-8197-2068ac993dec", AppName: "TestApplication"
	When AssignApp
	Then The claim AppsAuthenticated with value "TestApplication" was assigned to User "daca6f11-8ca8-4cd9-8197-2068ac993dec" successfully