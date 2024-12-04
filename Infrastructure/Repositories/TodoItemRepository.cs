using Domain.Model;
using Infrastructure.Context;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TodoItemRepository(ApplicationDbContext context) : GenericRepository<TodoItem>(context), ITodoItemRepository
{
    public async Task<ICollection<TodoItem>> GetPendingItems()
    {
        return await Find(item => !item.IsCompleted).ToListAsync();
    }
}
