Feature: Add User Claim

AddUserClaimCommandHandler: Add User Claim by UserName and Claim summary of the feature

@UserAcceptanceTest @AddUserClaimSuccessful
Scenario: Add User Claim Successful
	Given an AddUserClaimRequest with UserName "TestUser" with UserId: "daca6f11-8ca8-4cd9-8197-2068ac993dec", ClaimType: "Version", ClaimValue: "2.0.1"
	When AddUserClaim(Handle) is called
	Then the response should indicate successful claim added
