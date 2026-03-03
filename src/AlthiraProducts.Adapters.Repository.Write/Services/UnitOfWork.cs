using AlthiraProducts.Adapters.Repository.Write.Context;
using AlthiraProducts.Adapters.Repository.Write.Ports;
using Microsoft.EntityFrameworkCore;

namespace AlthiraProducts.Adapters.Repository.Write.Services;

public class UnitOfWork(ProductWriteContext context) : IUnitOfWork
{
    private readonly DbContext _context = context;

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
