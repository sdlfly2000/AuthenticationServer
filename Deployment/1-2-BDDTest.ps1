# Run Unit Tests, need to dotnet version 10
$TestProjects = @("../Application.Gateway.BDD/")

Write-Host "Running BDD Tests" -ForegroundColor DarkCyan

foreach($TestProject in $TestProjects) {
    if ((Get-Content -Path "devops.status") -ne "success") {
        exit $LASTEXITCODE
    }

    Write-Host "Running BDD Tests on $TestProject" -ForegroundColor Green
    dotnet test $TestProject --filter "TestCategory=UserAcceptanceTest"

    if ($LASTEXITCODE -ne 0) {
        Set-Content -Path "devops.status" -Value "error" -NoNewline
        exit $LASTEXITCODE
    }
}