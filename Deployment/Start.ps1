# Step 1: Build
Powershell.exe ./Build.ps1

# Step 2: Upload to Server
Powershell.exe ./Deploy.ps1

# Step 3: Restart Service
# ./RestartService.ps1

# Step 5: Stop running container
./StopDockerContainer.ps1
Start-Sleep -Seconds 2

# Step 3: Clear Existing Image
./ClearDockerImage.ps1
Start-Sleep -Seconds 2

# Step 4: Build Image
./BuildDockerImage.ps1
Start-Sleep -Seconds 2

# Step 6: Run container
./RunDockerImage.ps1
Start-Sleep -Seconds 2