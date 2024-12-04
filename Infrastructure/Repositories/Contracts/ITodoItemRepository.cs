using Domain.Model;
using Infrastructure.Repositories.Shared.Contracts;

namespace Infrastructure.Repositories.Contracts;

public interface ITodoItemRepository : IGenericRepository<TodoItem>
{
    Task<ICollection<TodoItem>> GetPendingItems();
}

