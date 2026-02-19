using Common.Core.DependencyInjection;
using Domain.Authorizations.Entities;
using Domain.Authorizations.Repositories;
using Infra.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Repositories;

[ServiceLocate(typeof(IRoleRepository))]
public class RoleRepository(IdDbContext context) : IRoleRepository
{
    private readonly IdDbContext _context = context;

    public async Task<Role> Add(Role role, CancellationToken cancellationToken)
    {
        return (await _context.Set<Role>().AddAsync(role, cancellationToken).ConfigureAwait(false)).Entity;
    }

    public async Task<Role> Get(Guid id, CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>()
            .Include(r => r.Rights)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken)
            .ConfigureAwait(false);

        return role ?? throw new DomainNotFoundException(nameof(Role), id.ToString());
    }

    public Role Update(Role role)
    {
        return _context.Set<Role>().Update(role).Entity;
    }

    public async Task<int> Save(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public void Delete(Role role)
    {
        _context.Remove(role);
    }

    public async Task<Role> GetByRoleName(string roleName, CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>()
                                .Include(r => r.Rights)
                                .SingleOrDefaultAsync(r => r.RoleName == roleName, cancellationToken)
                                .ConfigureAwait(false);

        DomainNotFoundException.ThrowIfNull(role, nameof(Role), roleName);

        return role;
    }

    public async Task<IList<Role>> GetByRoleNames(IList<string> roleNames, CancellationToken cancellationToken)
    {
        var roles = await _context.Set<Role>()
                                .Include(r => r.Rights)
                                .Where(r => roleNames.Contains(r.RoleName))
                                .ToListAsync(cancellationToken);

        DomainNotFoundException.ThrowIfNull(roles, nameof(Role), string.Join(',', roleNames));

        return roles;
    }
}
