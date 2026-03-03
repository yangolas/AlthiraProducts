namespace AlthiraProducts.Adapters.Repository.Write.Ports;

public interface IUnitOfWork : ITransientRepositoryWrite
{
    Task SaveChangesAsync(CancellationToken ct = default);
}