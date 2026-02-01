#!/bin/bash

if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
    exit $?
fi

echo "---UnZip AuthService Remotely---"

# Unzip deploy folder -- Install-Module -Name Posh-SSH
Password="sdl@1215"
User="devops"
ComputerNames=("homeserver2")

Command="echo $Password | sudo -S unzip -o /home/devops/Projects/AuthenticationService/AuthService.zip -d /home/devops/Projects/AuthenticationService/"

for ComputerName in "${ComputerNames[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

    echo "UnZip AuthService Remotely on $ComputerName"

    sshpass -p "$Password" ssh -o StrictHostKeyChecking=no "$User@$ComputerName" "$Command"

    echo

    if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done

