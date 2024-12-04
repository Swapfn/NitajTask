using Application.Mapper;
using Application.Services.Contracts;
using Domain.Dtos;
using Domain.Shared;
using FluentValidation;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Shared.Contracts;

namespace Application.Services;
public class TodoItemService(ITodoItemRepository repository, IUnitOfWork unitOfWork, IValidator<CreateTodoItemDto> validator) : ITodoItemService
{
    private readonly TodoItemMapper mapper = new();

    public async Task<Result> AddTodoItem(CreateTodoItemDto itemDto)
    {
        var result = await validator.ValidateAsync(itemDto);
        if (result.IsValid)
        {
            var item = await repository.AddAsync(mapper.CreateTodoItemDtoToTodoItem(itemDto));
            await unitOfWork.SaveChangesAsync();
            return new Result<TodoItemDto>(true, mapper.TodoItemToTodoItemDto(item));
        }
        else
            return new Result(false, result.Errors.Select(e => new Error(e.ErrorMessage)).ToList());
    }

    public async Task<Result> GetTodoItems()
    {
        var items = await repository.GetAllAsync();
        return new Result<List<TodoItemDto>>(true, mapper.TodoItemsToTodoItemDtos([.. items]));
    }

    public async Task<Result> GetPendingTodoItems()
    {
        var items = await repository.GetPendingItems();
        return new Result<List<TodoItemDto>>(true, mapper.TodoItemsToTodoItemDtos([.. items]));
    }

    public async Task<Result> Complete(int id)
    {
        var todoItem = await repository.GetByIdAsync(id);
        if (todoItem is not null && !todoItem.IsCompleted)
        {
            todoItem.IsCompleted = true;
            await repository.UpdateAsync(todoItem);
            await unitOfWork.SaveChangesAsync();
            return new Result<TodoItemDto?>(true, mapper.TodoItemToTodoItemDto(todoItem));
        }
        else if (todoItem is not null && todoItem.IsCompleted)
        {
            return new Result<TodoItemDto?>(true, mapper.TodoItemToTodoItemDto(todoItem));
        }
        return new Result(false, ["Todo Item Not Found"]);
    }
}
