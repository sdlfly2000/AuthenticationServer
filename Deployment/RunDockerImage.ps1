# Restart AuthService -- Install-Module -Name Posh-SSH
Write-Host "Run Service authservice:last docker image" -ForegroundColor DarkCyan
$Password = "sdl@1215"
$User = "sdlfly2000"
$ComputerName = "homeserver"
$Command = "sudo docker stack deploy -c /home/sdlfly2000/Projects/AuthenticationService/AuthService/docker-compose.yml AuthService"
$ExpectedString = "[sudo] password for " + $User + ":"

$secpasswd = ConvertTo-SecureString $Password -AsPlainText -Force
$Credentials = New-Object System.Management.Automation.PSCredential($User, $secpasswd)
$SessionID = New-SSHSession -ComputerName $ComputerName -Credential $Credentials #Connect Over SSH
$stream = $SessionID.Session.CreateShellStream("PS-SSH", 0, 0, 0, 0, 1000)
$result = Invoke-SSHStreamExpectSecureAction -ShellStream $stream -Command $Command -ExpectString $ExpectedString -SecureAction $secpasswd

Write-Host "Restarted: "$result -ForegroundColor DarkCyan

