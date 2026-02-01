#!/bin/bash
if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
    exit $?
fi

echo "---Uploading AuthenticationService---"

Password="sdl@1215"
User="devops"

fileName="AuthService.zip"
source="../Build/$fileName"
ftpHosts=("homeserver2")
urlDest="Projects/AuthenticationService"

for ftpHost in "${ftpHosts[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

	echo "Uploading AuthenticationService $ftpHost"
	
	FTPClient upload --host $ftpHost --userName $User --password $Password --remotePath "$urlDest/$fileName" --localFile $source

    echo

    if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done