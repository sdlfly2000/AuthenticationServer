# /user/bin/bash

## Setup version from file
version=$(</home/devops/Projects/AuthenticationService/version)

export AUTH_SERVICE_VERSION=${version}

## Update/Deploy to Kubernetes Cluster
envsubst < /home/devops/Projects/AuthenticationService/k8s-deployment.yaml | kubectl apply -f -