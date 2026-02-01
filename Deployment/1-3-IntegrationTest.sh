#!/bin/bash

TestProjects=("../Tests/IntegrationTests/Application.Service.AutomationTest/")

echo "---Running Integration Tests---"

for TestProject in "${TestProjects[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

    echo "Running Integration Tests on $TestProject"
    
    dotnet test "$TestProject" --filter "TestCategory=IntegrationTest"

    echo

    if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done