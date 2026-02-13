$datetime = Get-Date -Format "yyyy.MM.dd.HHmm"

# Build AngularPresentation
Write-Host "Building AuthService - WebUI" -ForegroundColor DarkCyan
pushd ../1-Presentation/WebUI/
ng build --configuration=production --output-path ../../Build/AuthService/wwwroot --delete-output-path
popd

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}

# Build AuthService
Write-Host "Building AuthService" -ForegroundColor DarkCyan
pushd ../1-Presentation/AuthService/
dotnet build --configuration release -p Version=$datetime --output ../../Build/AuthService/
popd

# Version
Set-Content -Path "../Build/AuthService/version" -Value $datetime -NoNewline

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}