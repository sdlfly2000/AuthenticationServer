#!/bin/bash

echo "success" > devops.status

# Step Build
bash ./1-Build.sh
sleep 2

# Run Unit Tests
bash ./1-1-UnitTest.sh
sleep 2

# Run BDD Tests
bash ./1-2-BDDTest.sh
sleep 2

# Run Integration Tests
bash ./1-3-IntegrationTest.sh
sleep 2

# step Zip Buid
bash ./2-ZipBuilt.sh
sleep 2

# step Clean Up deployment folder
bash ./3-CleanUpDeployFolder.sh
sleep 2

# step Deploy zip file
bash ./4-Deploy.sh
sleep 2

# step unzip file
bash ./5-UnZip.sh
sleep 2

# step build and push image
bash ./6-ExecuteImageBuildPush.sh
sleep 2

# step k8s deploy
bash ./7-ExecuteDeployImage.sh
sleep 2

rm ./devops.status