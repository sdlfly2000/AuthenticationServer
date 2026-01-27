#!/bin/bash

echo "---Running Unit Tests---"

# Run Unit Tests
TestProjects=("../Tests/UnitTests/Application.Services.Tests/" "../Tests/UnitTests/Domain.Tests/" "../Tests/UnitTests/Infra.Database.Tests/")

for TestProject in "${TestProjects[@]}"; do
    if [[ -f devops.status ]] && [[ $(cat devops.status) != "success" ]]; then
        exit $?
    fi

    echo "Running Unit Tests on $TestProject"
    dotnet test "$TestProject" --filter "TestCategory=UnitTest"

    if [[ $? -ne 0 ]]; then
        echo "error" > devops.status
        exit $?
    fi
done