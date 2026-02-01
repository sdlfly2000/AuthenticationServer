#!/bin/bash

if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
    exit $?
fi

echo "---Zip Artifacts---"

cd ../Build/AuthService/
zip -r ../AuthService.zip ./*
cd ../../Deployment

echo

if [[ $? -ne 0 ]]; then
    echo "error" > devops.status
    exit $?
fi