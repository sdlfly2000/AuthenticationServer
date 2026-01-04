# Application.Service.AddUserClaimCommandHandler

```mermaid
---
config:
    layout: elk
---

flowchart TB 

    start_overall((start))

    subgraph mainprocess[<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:70em'>Application.Service.AddUserClaimCommandHandler</div>]
        direction TB            

        subgraph subprocess0 [<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:58em'>Infra.Database.Repositories.UserRepository.Find</div>]
            direction TB
            
            FindUser["Find ***User*** By ***UserReference***"] -->
            check1{Found and Single?} 
        end       

        subgraph subprocess1 [<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:58em'>Domain.User.Entities.User.AddClaim</div>]
            direction TB
            userFound{"Duplicated ***Name*** of ***Claim*** in ***User***?"}
            userFound --"no"-->       
            AddClaim["Add ***Claim*** to ***User***"]    
        end

        subgraph subprocess2 [<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:58em'>Infra.Database.Persistors.UserPersistor.Update</div>]
            direction TB
            updateDatabase["Persist/Update to Database"]
        end

        return["return AddUserClaimResponse"]
    end

    subgraph mainExceptions [<div style='display:flex; justify-content:flex-start; align-items:flex-start;width:60em'>Exceptions</div>]
        direction TB
        UserNotFound["Throw Not Found / Not Single Exception"]
        DuplicatedClaim["Throw duplicated Exception [InvalidOperationException]"] 
    end

    end_overall(end)    

    %%{Class Relationship}%%
    start_overall --> FindUser
    return --> end_overall
    check1 --"yes"--> userFound
    AddClaim --> updateDatabase
    updateDatabase --> return

    check1 --"no"--> UserNotFound
    userFound --"yes"--> DuplicatedClaim

    %% Custom Styles
    style mainprocess stroke-dasharray: 5 5;
    style mainExceptions stroke-dasharray: 5 5;
    style subprocess0 stroke-dasharray: 5 5;
    style subprocess1 stroke-dasharray: 5 5;
    style subprocess2 stroke-dasharray: 5 5;

```

---

```mermaid
---
config:
    class:
        hideEmptyMembersBox: true
---

classDiagram
    %%{Class Definition}%%

    class AppReqeuest {
        <<abstract>>
    }

    class AppResponse {
        <<abstract>>
        + ErrorMessage: string
        + Success: bool
    }

    class AddUserClaimCommandHandler {
        - userPersistor: IUserPersistor
        - userRepository: IUserRepository
        - serviceProvider: IServiceProvider
        + Handle(request: AddUserClaimRequest): AddUserClaimResponse
    }

    class AddUserClaimRequest { 
        + UserId: string
        + ClaimType: string
        + ClaimValue: string
    }

    class AddUserClaimResponse {
    }

    %%{Class Relationship}%%
    AddUserClaimCommandHandler --  AddUserClaimRequest : use
    AddUserClaimCommandHandler --  AddUserClaimResponse : use
    AppReqeuest <|-- AddUserClaimRequest
    AppResponse <|-- AddUserClaimResponse

```
