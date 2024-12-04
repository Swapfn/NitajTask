using Domain.Dtos;
using Domain.Shared;

namespace Application.Services.Contracts;
public interface ITodoItemService
{
    Task<Result> AddTodoItem(CreateTodoItemDto itemDto);
    Task<Result> Complete(int id);
    Task<Result> GetPendingTodoItems();
    Task<Result> GetTodoItems();
}
