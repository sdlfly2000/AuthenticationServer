# Step Build
Powershell.exe ./Build.ps1

# step Zip Buid
./ZipBuilt.ps1

# step Clean Up deployment folder
./CleanUpDeployFolder.ps1
Start-Sleep -Seconds 2

# step Deploy zip file
./Deploy.ps1
Start-Sleep -Seconds 2

# step unzip file
./UnZip.ps1
Start-Sleep -Seconds 2

# step remove zip file
./RemoveZip.ps1
Start-Sleep -Seconds 2

# Step: Stop running container
./StopDockerContainer.ps1
Start-Sleep -Seconds 2

# Step: Clear Existing Image
./ClearDockerImage.ps1
Start-Sleep -Seconds 2

# Step: Build Image
./BuildDockerImage.ps1
Start-Sleep -Seconds 2

# Step 6: Run container
./RunDockerImage.ps1
Start-Sleep -Seconds 2