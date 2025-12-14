# Run Unit Tests
$TestProjects = @("../Tests/UnitTests/Application.Services.Tests/", "../Tests/UnitTests/Domain.Tests/", "../Tests/UnitTests/Infra.Database.Tests/")

Write-Host "Running Unit Tests" -ForegroundColor DarkCyan

foreach($TestProject in $TestProjects) {
    if ((Get-Content -Path "devops.status") -ne "success") {
        exit $LASTEXITCODE
    }

    Write-Host "Running Unit Tests on $TestProject" -ForegroundColor Green
    dotnet test $TestProject --filter "TestCategory=UnitTest"

    if ($LASTEXITCODE -ne 0) {
        Set-Content -Path "devops.status" -Value "error" -NoNewline
        exit $LASTEXITCODE
    }
}