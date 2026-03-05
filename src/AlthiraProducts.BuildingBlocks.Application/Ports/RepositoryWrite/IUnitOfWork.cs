using AlthiraProducts.BuildingBlocks.Application.Ports.ServiceRegistration;

namespace AlthiraProducts.BuildingBlocks.Application.Ports.RepositoryWrite;

public interface IUnitOfWork : ITransient
{
    Task SaveChangesAsync(CancellationToken ct = default);
}