Set-Content -Path "devops.status" -Value "success" -NoNewline

# Step Build
./1-Build.ps1
Start-Sleep -Seconds 2

# Run Unit Tests
./1-1-UnitTest.ps1
Start-Sleep -Seconds 2

# Run BDD Tests
./1-2-BDDTest.ps1
Start-Sleep -Seconds 2

# Run Integration Tests
./1-3-IntegrationTest.ps1
Start-Sleep -Seconds 2

# step Zip Buid
./2-ZipBuilt.ps1
Start-Sleep -Seconds 2

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

Remove-Item -Path "devops.status"