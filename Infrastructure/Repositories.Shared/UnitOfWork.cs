using Infrastructure.Context;
using Infrastructure.Repositories.Shared.Contracts;

namespace Infrastructure.Repositories.Shared;
public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public virtual async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public virtual void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}