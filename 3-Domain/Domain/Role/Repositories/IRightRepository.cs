namespace Domain.Role.Repositories;

public interface IRightRepository
{
    Task<Entities.Right> Get(Guid id, CancellationToken cancellationToken);

    Task<int> Delete(Entities.Right role, CancellationToken cancellationToken);

    Task<int> Save(CancellationToken cancellationToken);
}
