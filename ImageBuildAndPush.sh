# /user/bin/bash

## Setup version from file
version=$(</home/devops/Projects/AuthenticationService/version)

## Clean zip file after unzip
rm /home/devops/Projects/AuthenticationService/AuthService.zip

## Build Docker Image for Kubernetes Deployment
docker image build -t registry.activator.com/authservice/authservice:$version /home/devops/Projects/AuthenticationService/

## Push Docker Image to Private Registry
docker push registry.activator.com/authservice/authservice:$version

## Executable to DeployImage.sh
chmod +x /home/devops/Projects/AuthenticationService/DeployImage.sh
