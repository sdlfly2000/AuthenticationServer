# Build AngularPresentation
Write-Host "Building AngularPresentation" -ForegroundColor DarkCyan
pushd ../AngularPresentation/
ng build --configuration=production --output-path ../Build/AuthService/wwwroot 
popd

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}

# Build AuthService
Write-Host "Building AuthService" -ForegroundColor DarkCyan
pushd ../AuthService/
dotnet build --configuration release --output ../Build/AuthService/
popd

# Version
$datetime = Get-Date -Format "yyyyMMdd-HHmmss"
Set-Content -Path "../Build/AuthService/version" -Value $datetime -NoNewline

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}