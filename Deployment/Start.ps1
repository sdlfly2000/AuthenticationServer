# Step Build
Powershell.exe ./1-Build.ps1

# step Zip Buid
./2-ZipBuilt.ps1

# step Clean Up deployment folder
./3-CleanUpDeployFolder.ps1
Start-Sleep -Seconds 2

# step Deploy zip file
./4-Deploy.ps1
Start-Sleep -Seconds 2

# step unzip file
./5-UnZip.ps1
Start-Sleep -Seconds 2

# step remove zip file
./6-RemoveZip.ps1
Start-Sleep -Seconds 2

# Step: Stop running container
./7-StopDockerContainer.ps1
Start-Sleep -Seconds 2

# Step: Clear Existing Image
./8-ClearDockerImage.ps1
Start-Sleep -Seconds 2

# Step: Build Image
./9-BuildDockerImage.ps1
Start-Sleep -Seconds 2

# Step 6: Run container
./10-RunDockerImage.ps1
Start-Sleep -Seconds 2