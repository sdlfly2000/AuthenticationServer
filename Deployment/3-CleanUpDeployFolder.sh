#!/bin/bash

if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
    exit $?
fi

echo "---Cleanup Remote Deploy Folder---"

Password="sdl@1215"
User="devops"

ComputerNames=("homeserver2")

Command="echo $Password | sudo -S rm -r /home/devops/Projects/AuthenticationService/*"

for ComputerName in "${ComputerNames[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

	echo "Cleaning up deploy folder on $ComputerName"

	sshpass -p "$Password" ssh -o StrictHostKeyChecking=no "$User@$ComputerName" "$Command"

    echo

    if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done
