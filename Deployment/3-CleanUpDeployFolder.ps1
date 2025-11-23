if ((Get-Content -Path "devops.status") -ne "success") {
    exit $LASTEXITCODE
}

# Cleanup deploy folder -- Install-Module -Name Posh-SSH
Write-Host "Cleanup deploy folder" -ForegroundColor DarkCyan
$Password = "sdl@1215"
$User = "devops"
$ComputerNames = @("homeserver2")
$Command = "sudo rm -r /home/devops/Projects/AuthenticationService/*"
$ExpectedString = "[sudo] password for " + $User + ":"

$secpasswd = ConvertTo-SecureString $Password -AsPlainText -Force
$Credentials = New-Object System.Management.Automation.PSCredential($User, $secpasswd)
foreach($ComputerName in $ComputerNames){
	$SessionID = New-SSHSession -ComputerName $ComputerName -Credential $Credentials #Connect Over SSH
	$stream = $SessionID.Session.CreateShellStream("PS-SSH", 0, 0, 0, 0, 1000)
	$result = Invoke-SSHStreamExpectSecureAction -ShellStream $stream -Command $Command -ExpectString $ExpectedString -SecureAction $secpasswd
	$stream.Read()
}

if ($LASTEXITCODE -ne 0) {
    Set-Content -Path "devops.status" -Value "error" -NoNewline
    exit $LASTEXITCODE
}
