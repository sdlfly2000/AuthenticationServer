# Run Integration Tests
$TestProjects = @("../Application.Service.AutomationTest/")

Write-Host "Running Integration Tests" -ForegroundColor DarkCyan

foreach($TestProject in $TestProjects) {
    if ((Get-Content -Path "devops.status") -ne "success") {
        exit $LASTEXITCODE
    }

    Write-Host "Running Integration Tests on $TestProject" -ForegroundColor Green
    dotnet test $TestProject --filter "TestCategory=IntegrationTest"

    if ($LASTEXITCODE -ne 0) {
        Set-Content -Path "devops.status" -Value "error" -NoNewline
        exit $LASTEXITCODE
    }
}