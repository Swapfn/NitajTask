using Domain.Dtos;
using Domain.Model;
using Riok.Mapperly.Abstractions;

namespace Application.Mapper;

[Mapper]
public partial class TodoItemMapper
{
    public partial TodoItem CreateTodoItemDtoToTodoItem(CreateTodoItemDto item);
    public partial List<TodoItemDto> TodoItemsToTodoItemDtos(List<TodoItem> items);
    public partial TodoItemDto TodoItemToTodoItemDto(TodoItem items);
}
