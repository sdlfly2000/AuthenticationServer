#!/bin/bash

TestProjects=("../Tests/AcceptanceTests/Application.Gateway.BDD/" "../Tests/AcceptanceTests/Application.Services.BDD/")

echo "---Running BDD Tests---"


for TestProject in "${TestProjects[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

    echo "Running BDD Tests on $TestProject"
    dotnet test "$TestProject" --filter "TestCategory=UserAcceptanceTest"

    if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done