# Application.Gateway.User - UserGateway.Register

```mermaid
---
title: "Procedure: User Registartion Procedure"
---

flowchart TB
    start_overall((start))

    subgraph user_register[<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:50em'>Application.Gateway.User.UserGateway.Register</div>]

        subgraph password_extraction_verification[<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:48em'>PasswordHelper.ExtractPwdWithTimeVerification</div>]
            direction TB
            PwdExtraction[Password Extraction and Time Verfication]
        end

        subgraph insert_user_database[<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:48em'>Application.Services.User.Commands.RegisterUserCommandHandler</div>]
            direction TB
            InsertUser["`Insert ***User*** with Repository`"]
            PublishUserMessage["`Publish ***UserRegisterdEvent*** to MessageBus`"]
        end
    end

    end_overall(end)

    %%{relationship}%%
    start_overall --> PwdExtraction -->
    InsertUser --> PublishUserMessage -->
    end_overall

    %% Custom Styles
    style user_register stroke-dasharray: 5 5;
    style password_extraction_verification stroke-dasharray: 5 5;
    style insert_user_database stroke-dasharray: 5 5;        
```
