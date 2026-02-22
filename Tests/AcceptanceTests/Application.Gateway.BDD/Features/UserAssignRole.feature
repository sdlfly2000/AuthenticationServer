Feature: User Role Assignment

User Roles Assignment

Background: 
	Given Connect to Database "IdentityTest"

@UserAcceptanceTest @UserAssignRoleSuccessful
Scenario: User Role Assignment Successful
	Given User with UserId: "daca6f11-8ca8-4cd9-8197-2068ac993dec" with Claim AppName: "TestApplication"
	Given RoleName: "TestRole"
	When AssignRole
	Then The claim role "TestRole" was assigned to User "daca6f11-8ca8-4cd9-8197-2068ac993dec" successfully