# Common Function to Deploy Project
function UploadProject(){
	param(
	[string]$sourceFile,
	[string]$fileName,
	[string]$ftpHost,
	[string]$projectName,
	[string]$username = "devops",
	[string]$password = "sdl@1215",
	[string]$urlDest = "Projects/AuthenticationService"
	# [string]$urlDestination = "ftp://homeserver2/Projects/AuthenticationService"
	)

	try{
		Write-Host "Uploading File: $sourceFile" -ForegroundColor DarkCyan
		FTPClient.exe upload --host $ftpHost --userName $username --password $password --remotePath "$urlDest/$fileName" --localFile $sourceFile
		
		Write-Host
		Write-Host "Uploaded File: $sourceFile Successful to $urlDest/$fileName" -ForegroundColor DarkCyan
	}
	catch {
		write-Error "An unexpected error occurred: $($_.Exception.Message)"
	}
}

if ((Get-Content -Path "devops.status") -ne "success") {
    exit $LASTEXITCODE
}

# Upload AuthService - existing Directory
Write-Host "Uploading AuthenticationService" -ForegroundColor DarkCyan
$fileName="AuthService.zip"
$source = "../Build/$fileName"
$ftpHosts = @("homeserver2")
$urlDest= "Projects/AuthenticationService"

foreach($ftpHost in $ftpHosts){
	UploadProject -fileName $fileName -sourceFile $source -ftpHost $ftpHost -urlDestination $urlDest
}

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}