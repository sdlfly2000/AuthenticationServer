if ((Get-Content -Path "devops.status") -ne "success") {
    exit $LASTEXITCODE
}

# Unzip deploy folder -- Install-Module -Name Posh-SSH
Write-Host "UnZip AuthService" -ForegroundColor DarkCyan
$Password = "sdl@1215"
$User = "devops"
$ComputerNames = @("homeserver2")
$Command = 'sudo unzip -o /home/devops/Projects/AuthenticationService/AuthService.zip -d /home/devops/Projects/AuthenticationService/'
$ExpectedString = "[sudo] password for " + $User + ":"

$secpasswd = ConvertTo-SecureString $Password -AsPlainText -Force
$Credentials = New-Object System.Management.Automation.PSCredential($User, $secpasswd)
foreach($ComputerName in $ComputerNames){
    $SessionID = New-SSHSession -ComputerName $ComputerName -Credential $Credentials #Connect Over SSH
    $stream = $SessionID.Session.CreateShellStream("PS-SSH", 0, 0, 0, 0, 1000)
    $result = Invoke-SSHStreamExpectSecureAction -ShellStream $stream -Command $Command -ExpectString $ExpectedString -SecureAction $secpasswd
    Write-Host "UnZip AuthService on $ComputerName : $result" -ForegroundColor DarkCyan
    $stream.Read()
    Start-Sleep -Seconds 2
}

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}

