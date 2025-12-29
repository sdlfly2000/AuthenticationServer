Feature: Delete User Claim

DeleteUserClaimCommandHandler: Delete User Claim by UserName and Claim

@UserAcceptanceTest
Scenario: Delete User Claim Successful
	Given an DeleteUserClaimRequest with UserName "TestUser" with UserId: "90bcee8a-5120-4453-afad-6aeff61aa6f9", ClaimType: "Version", ClaimValue: "2.0.1"
	When DeleteUserClaim(Handle) is called
	Then the response should indicate successful claim deletion