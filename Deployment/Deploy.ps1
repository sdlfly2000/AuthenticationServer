# Common Function to Deploy Project
function UploadProject(){
	param(
	[string]$sourceFolder,
	[string]$projectName,
	[string]$username = "sdlfly2000",
	[string]$password = "sdl@1215",
	[string]$urlDestination = "ftp://192.168.71.246/Projects/AuthenticationService/"
	)
	$webclient = New-Object -TypeName System.Net.WebClient
	$webclient.Credentials = New-Object System.Net.NetworkCredential($username,$password)

	$files = Get-ChildItem $sourceFolder

	foreach ($file in $files)
	{
		Write-Host "Uploading $file"
		$webclient.UploadFile("$urlDestination/$projectName/$file", $file.FullName)
	} 

	$webclient.Dispose()
}

# Upload AngularPresentation
Write-Host "Uploading AngularPresentation" -ForegroundColor DarkCyan
$source = "../Build/AngularPresentation"
$projectName= "AngularPresentation"

UploadProject -sourceFolder $source -projectName $projectName

# Upload AuthService
Write-Host "Uploading AuthService" -ForegroundColor DarkCyan
$source = "../Build/AuthService"
$projectName= "AuthService"

UploadProject -sourceFolder $source -projectName $projectName