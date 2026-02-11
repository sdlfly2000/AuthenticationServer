#!/bin/bash

DATETIME=$(date +"%Y.%m.%d.%H%M")

# Build AngularPresentation
echo "---Building AuthService - WebUI---"
cd ../1-Presentation/WebUI/
ng build --configuration=production --output-path ../../Build/AuthService/wwwroot --delete-output-path || (echo "error" > ../../Deployment/devops.status; exit 1)
cd ../../Deployment

# Build AuthService
echo "---Building AuthService---"

cd ../1-Presentation/AuthService/
dotnet build --configuration release -p Version=$DATETIME --output ../../Build/AuthService/ || (echo "error" > ../../Deployment/devops.status; exit 1)
cd ../../Deployment

echo

# Version
echo "---Versioning AuthService---"
echo "Build timestamp: $DATETIME"
$(echo $DATETIME > ../Build/AuthService/version) || (echo "error" > ../../Deployment/devops.status; exit 1)