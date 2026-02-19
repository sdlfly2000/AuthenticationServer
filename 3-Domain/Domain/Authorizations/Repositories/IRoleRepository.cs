using Domain.Authorizations.Entities;

namespace Domain.Authorizations.Repositories;

public interface IRoleRepository
{
    Task<Role> Get(Guid id, CancellationToken cancellationToken);

    Task<Role> GetByRoleName(string roleName, CancellationToken cancellationToken);

    Task<IList<Role>> GetByRoleNames(IList<string> roleNames, CancellationToken cancellationToken);

    Task<Role> Add(Role role, CancellationToken cancellationToken);

    Role Update(Role role);

    void Delete(Role role);

    Task<int> Save(CancellationToken cancellationToken);
}
