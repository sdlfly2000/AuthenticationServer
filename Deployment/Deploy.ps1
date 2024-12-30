# Common Function to Deploy Project
function UploadProject(){
	param(
	[string]$sourceFolder,
	[string]$projectName,
	[string]$username = "sdlfly2000",
	[string]$password = "sdl@1215",
	[string]$urlDestination = "ftp://homeserver/Projects/AuthenticationService"
	)
	$webclient = New-Object -TypeName System.Net.WebClient
	$webclient.Credentials = New-Object System.Net.NetworkCredential($username,$password)

	$files = Get-ChildItem $sourceFolder

	foreach ($file in $files)
	{
        if ($file.PSIsContainer -eq $true) {
            Write-Host "Uploading Folder: $file"
            UploadProject -sourceFolder "$sourceFolder/$file" -projectName "$projectName/$file"

        }else{
            Write-Host "Uploading File: $projectName/$file"
		    $webclient.UploadFile("$urlDestination/$projectName/$file", $file.FullName)
        }
	}

	$webclient.Dispose()
}

# Upload AuthService - existing Directory
Write-Host "Uploading AuthService" -ForegroundColor DarkCyan
$source = "../Build/AuthService"
$projectName= "AuthService"

UploadProject -sourceFolder $source -projectName $projectName