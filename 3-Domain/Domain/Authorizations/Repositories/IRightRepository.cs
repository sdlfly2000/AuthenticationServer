using Domain.Authorizations.Entities;

namespace Domain.Authorizations.Repositories;

public interface IRightRepository
{
    Task<Right> Get(Guid id, CancellationToken cancellationToken);

    Task<int> Delete(Right role, CancellationToken cancellationToken);

    Task<int> Save(CancellationToken cancellationToken);
}
