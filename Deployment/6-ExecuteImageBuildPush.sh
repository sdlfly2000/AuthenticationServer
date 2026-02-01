#!/bin/bash

if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
    exit $?
fi

echo "---Execute Build and Push Image Script Remotely---"

Password="sdl@1215"
User="devops"
ComputerNames=("homeserver2")

Command="echo $Password | sudo -S bash /home/devops/Projects/AuthenticationService/ImageBuildAndPush.sh"

for ComputerName in "${ComputerNames[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

	echo "Execute Build and Push Image Script Remotely on $ComputerName"

	sshpass -p "$Password" ssh -o StrictHostKeyChecking=no "$User@$ComputerName" "$Command"

	echo

	if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done


