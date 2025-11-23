if ((Get-Content -Path "devops.status") -ne "success") {
    exit
}

# Zip the Artifacts
Write-Host "Zip Artifacts" -ForegroundColor DarkCyan
# Add-Type -AssemblyName System.IO.Compression.FileSystem 
# [System.IO.Compression.ZipFile]::CreateFromDirectory("../Artifacts/SimpleDashboard", "../Artifacts/SimpleDashboard.zip")
Compress-Archive -Path "../Build/AuthService/*" -DestinationPath "../Build/AuthService.zip" -Force

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}