# Domain - Role

```mermaid
---
config:
    class:
        hideEmptyMembersBox: true
---

classDiagram
    class Role {
        <<Entity>>
        + Id: Guid
        + RoleName: string
        + Rights: List~Right~
    }

    class Right {
        <<Entity>>
        + Id: Guid
        + RightName: string
    }

    %%{{Relationship}}%%
    Role --> "0..*" Right
```
