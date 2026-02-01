#!/bin/bash

if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
    exit $?
fi

echo "---Execute Image Script Remotely---"

Password="sdl@1215"
User="devops"
ComputerNames=("homeserver2")

Command="bash /home/devops/Projects/AuthenticationService/DeployImage.sh"

for ComputerName in "${ComputerNames[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

    echo "Execute Image Script Remotely on $ComputerName"

    sshpass -p "$Password" ssh -o StrictHostKeyChecking=no "$User@$ComputerName" "$Command"

    echo

    if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done