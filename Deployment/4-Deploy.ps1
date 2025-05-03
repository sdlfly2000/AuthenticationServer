# Common Function to Deploy Project
function UploadProject(){
	param(
	[string]$sourceFile,
	[string]$projectName,
	[string]$username = "sdlfly2000",
	[string]$password = "sdl@1215",
	[string]$urlDestination = "ftp://homeserver2/Projects/AuthenticationService"
	)
	$webclient = New-Object -TypeName System.Net.WebClient
	$webclient.Credentials = New-Object System.Net.NetworkCredential($username,$password)

	Write-Host "Uploading File: $sourceFile" -ForegroundColor DarkCyan
	$webclient.UploadFile("$urlDestination/AuthService.zip", $sourceFile)

    Write-Host "Uploaded File: $sourceFile Successful to $urlDestination" -ForegroundColor DarkCyan
	$webclient.Dispose()
}

# Upload AuthService - existing Directory
Write-Host "Uploading AuthenticationService" -ForegroundColor DarkCyan
$source = "../Build/AuthService.zip"
$urlDests= @("ftp://homeserver2/Projects/AuthenticationService","ftp://homeserver/Projects/AuthenticationService")

foreach($urlDest in $urlDests){
	UploadProject -sourceFile $source -urlDestination $urlDest
}