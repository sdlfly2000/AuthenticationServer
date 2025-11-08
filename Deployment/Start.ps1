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

# step build and push image
./6-ExecuteImageBuildPush.ps1
Start-Sleep -Seconds 2

# step k8s deploy
./7-ExecuteDeployImage.ps1
Start-Sleep -Seconds 2
