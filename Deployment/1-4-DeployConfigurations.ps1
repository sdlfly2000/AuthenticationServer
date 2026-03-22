# Deploying Configurations
param(
	[string]$tempDirectory = ".",
	[string]$username = "sdlfly2000",
	[string]$url = "ftp://homeserver2/Projects/Configurations",
	[string]$configFileName = "AuthenticationService.json"
)

################################################
# Apply variables to appsettings.{env}.json
################################################
function ApplyVariables {
    param (
        [Parameter(Mandatory=$true)]
        [PSCustomObject]$env
    )
    
    $targetFile = "../Build/AuthService/appsettings.$($env.name).json"
    Write-Host "Environment: $($env.name) -> $targetFile" -ForegroundColor DarkCyan
    $content = Get-Content -Path $targetFile -Raw
    
    foreach ($var in $env.variables.PSObject.Properties) {
        $content = $content -replace "{{$($var.Name)}}", $var.Value
    }
    
    Set-Content -Path $targetFile -Value $content
}
################################################

Write-Host "Deploying Configurations" -ForegroundColor DarkCyan

# Download file
$webclient = New-Object -TypeName System.Net.WebClient
$webclient.Credentials = New-Object System.Net.NetworkCredential($username, "sdl@1215")

$uri = New-Object System.Uri("$url/$configFileName")

Write-Host "****************************************"
Write-Host "url : $url"
Write-Host "configFileName : $configFileName"
Write-Host "****************************************"

Write-Host "Download File $url/$configFileName to $tempDirectory"
$webclient.DownloadFile($uri, "$tempDirectory/$configFileName")

$webclient.Dispose()

$config = Get-Content -Path "$tempDirectory/$configFileName" | ConvertFrom-Json

foreach ($env in $config.environment) {
    ApplyVariables -env $env
}

# Clean up
Remove-Item ".\$configFileName"

Write-Host "Complete Deploying Configurations" -ForegroundColor DarkCyan