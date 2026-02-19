using Common.Core.DependencyInjection;
using Domain.Authorizations.Entities;
using Domain.Authorizations.Repositories;
using Infra.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Repositories;

[ServiceLocate(typeof(IRightRepository))]
public class RightRepository(IdDbContext context) : IRightRepository
{
    private readonly IdDbContext _context = context;

    public async Task<Right> Get(Guid id, CancellationToken cancellationToken)
    {
        var right = await _context.Set<Right>()
            .SingleOrDefaultAsync(r => r.Id == id);

        return right is null
            ? throw new DomainNotFoundException(nameof(Right), id.ToString())
            : right;
    }

    public async Task<int> Delete(Right right, CancellationToken cancellationToken)
    {
        return await _context.Set<Right>()
                                .Where(r => r.Id == right.Id)
                                .ExecuteDeleteAsync(cancellationToken)
                                .ConfigureAwait(false); // Execute immediately in the database
    }

    public async Task<int> Save(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
