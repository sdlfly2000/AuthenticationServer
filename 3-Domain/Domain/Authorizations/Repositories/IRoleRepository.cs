namespace Domain.Authorizations.Repositories;

public interface IRoleRepository
{
    Task<Entities.Role> Get(Guid id, CancellationToken cancellationToken);

    Task<Entities.Role> GetByRoleName(string roleName, CancellationToken cancellationToken);

    Task<Entities.Role> Add(Entities.Role role, CancellationToken cancellationToken);

    Entities.Role Update(Entities.Role role);

    void Delete(Entities.Role role);

    Task<int> Save(CancellationToken cancellationToken);
}
