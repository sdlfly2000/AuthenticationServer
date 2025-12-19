# Common Function to Deploy Project
function UploadProject(){
	param(
	[string]$sourceFile,
	[string]$projectName,
	[string]$username = "devops",
	[string]$password = "sdl@1215",
	[string]$urlDestination = "ftp://homeserver2/Projects/AuthenticationService"
	)
	$webclient = New-Object -TypeName System.Net.WebClient
	$webclient.Credentials = New-Object System.Net.NetworkCredential($username,$password)

	try{
		Write-Host "Uploading File: $sourceFile" -ForegroundColor DarkCyan
		$webclient.UploadFile("$urlDestination/AuthService.zip", $sourceFile)

		Write-Host "Uploaded File: $sourceFile Successful to $urlDestination" -ForegroundColor DarkCyan
	}
	catch {
		write-Error "An unexpected error occurred: $($_.Exception.Message)"
	}
	finally{
		$webclient.Dispose()
	}
}

if ((Get-Content -Path "devops.status") -ne "success") {
    exit $LASTEXITCODE
}

# Upload AuthService - existing Directory
Write-Host "Uploading AuthenticationService" -ForegroundColor DarkCyan
$source = "../Build/AuthService.zip"
$urlDests= @("ftp://homeserver2/Projects/AuthenticationService")

foreach($urlDest in $urlDests){
	UploadProject -sourceFile $source -urlDestination $urlDest
}

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}