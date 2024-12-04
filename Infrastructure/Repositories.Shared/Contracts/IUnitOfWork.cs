namespace Infrastructure.Repositories.Shared.Contracts;

public interface IUnitOfWork : IDisposable
{
    Task SaveChangesAsync();
}
