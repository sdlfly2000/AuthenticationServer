using Common.Core.DependencyInjection;
using Domain.Authorizations.Entities;
using Domain.Authorizations.Repositories;
using Infra.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Repositories;

[ServiceLocate(typeof(IRoleRepository))]
public class RoleRepository : IRoleRepository
{
    private readonly IdDbContext _context;

    public RoleRepository(IdDbContext context)
    {
        _context = context;
    }

    public async Task<Role> Add(Role role, CancellationToken cancellationToken)
    {
        return (await _context.Set<Role>().AddAsync(role, cancellationToken).ConfigureAwait(false)).Entity;
    }

    public async Task<Role> Get(Guid id, CancellationToken cancellationToken)
    {
        var role = await _context.Set<Role>()
            .Include(r => r.Rights)
            .SingleOrDefaultAsync(r => r.Id == id);

        return role is null 
            ? throw new DomainNotFoundException(nameof(Role), id.ToString()) 
            : role;
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
                                .SingleOrDefaultAsync(r => r.RoleName.Equals(roleName, StringComparison.InvariantCultureIgnoreCase));

        DomainNotFoundException.ThrowIfNull(role, nameof(Role), roleName);

        return role;
    }
}
