# Install-Module -Name Posh-SSH
Write-Host "Execute Image Script Remotely" -ForegroundColor DarkCyan
$Password = "sdl@1215"
$User = "devops"
$ComputerNames = @("homeserver2")
$Command = "bash /home/devops/Projects/AuthenticationService/DeployImage.sh"

$secpasswd = ConvertTo-SecureString $Password -AsPlainText -Force
$Credentials = New-Object System.Management.Automation.PSCredential($User, $secpasswd)
foreach($ComputerName in $ComputerNames){
	$sshSession = New-SSHSession -ComputerName $ComputerName -Credential $Credentials #Connect Over SSH
	$result = Invoke-SSHCommand -SSHSession $sshSession -Command $Command
	$result.Output
}


