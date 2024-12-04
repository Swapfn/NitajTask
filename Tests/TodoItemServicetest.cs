using Application.Mapper;
using Application.Services;
using Application.Services.Contracts;
using Domain.Dtos;
using Domain.Model;
using Domain.Shared;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Shared.Contracts;
using NSubstitute;
using ValidationResult = FluentValidation.Results.ValidationResult;
namespace Tests;

public class TodoItemServicetest
{

    private readonly ITodoItemRepository _repositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IValidator<CreateTodoItemDto> _validatorMock;
    private readonly ITodoItemService _service;
    private readonly TodoItemMapper _mapperMock = new();
    public TodoItemServicetest()
    {
        _repositoryMock = Substitute.For<ITodoItemRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _validatorMock = Substitute.For<IValidator<CreateTodoItemDto>>();
        _service = new TodoItemService(_repositoryMock, _unitOfWorkMock, _validatorMock);
    }
    [Fact]
    public async Task AddTodoItem_NormalInput_Success()
    {
        // Arrange
        DateTime time = DateTime.UtcNow;
        var itemDto = new CreateTodoItemDto("Test", "Description");
        var todoItem = new TodoItem { Id = 1, Title = "Test", Description = "Description", IsCompleted = false, CreatedDate = time };
        var todoItemDto = new TodoItemDto(todoItem.Id, todoItem.Title, todoItem.Description, todoItem.IsCompleted, todoItem.CreatedDate);
        _validatorMock.ValidateAsync(itemDto).Returns(new ValidationResult());
        _repositoryMock.AddAsync(Arg.Any<TodoItem>()).Returns(todoItem);

        // Act
        var result = await _service.AddTodoItem(itemDto) as Result<TodoItemDto>;

        // Assert
        await _repositoryMock.Received(1).AddAsync(Arg.Any<TodoItem>());
        await _unitOfWorkMock.Received(1).SaveChangesAsync();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(todoItemDto, result.Data);
        Assert.Null(result.Errors);
    }
    [Fact]
    public async Task AddTodoItem_EmptyTitle_Failure()
    {
        // Arrange
        var itemDto = new CreateTodoItemDto("", "Description");
        _validatorMock.ValidateAsync(itemDto).Returns(new ValidationResult { Errors = { new ValidationFailure("Title", "Title cannot be empty.") } });

        // Act
        var result = await _service.AddTodoItem(itemDto);

        // Assert
        await _repositoryMock.Received(0).AddAsync(Arg.Any<TodoItem>());
        await _unitOfWorkMock.Received(0).SaveChangesAsync();
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotNull(result.Errors);
        Assert.Equal("Title cannot be empty.", result.Errors[0].Message);
    }

    [Fact]
    public async Task GetTodoItems_ReturnsItems()
    {
        // Arrange
        var todoItems = new List<TodoItem>
        {
            new() { Id = 1, Title = "Task 1", Description = "Description 1", IsCompleted = false, CreatedDate = DateTime.UtcNow },
            new() { Id = 2, Title = "Task 2", Description = "Description 2", IsCompleted = true, CreatedDate = DateTime.UtcNow }
        };

        _repositoryMock.GetAllAsync().Returns(todoItems);

        // Act
        var result = await _service.GetTodoItems() as Result<List<TodoItemDto>>;

        // Assert
        await _repositoryMock.Received(1).GetAllAsync();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetPendingTodoItems_ReturnsPendingItems()
    {
        // Arrange
        var pendingItems = new List<TodoItem>
        {
            new() { Id = 1, Title = "Pending Task", Description = "Pending Description", IsCompleted = false, CreatedDate = DateTime.UtcNow }
        };

        _repositoryMock.GetPendingItems().Returns(pendingItems);

        // Act
        var result = await _service.GetPendingTodoItems() as Result<List<TodoItemDto>>;

        // Assert
        await _repositoryMock.Received(1).GetPendingItems();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data);
    }

    [Fact]
    public async Task Complete_TodoItemExistsAndNotCompleted_Success()
    {
        // Arrange
        var todoItem = new TodoItem { Id = 1, Title = "Task", Description = "Description", IsCompleted = false, CreatedDate = DateTime.UtcNow };
        _repositoryMock.GetByIdAsync(todoItem.Id).Returns(todoItem);

        // Act
        var result = await _service.Complete(todoItem.Id) as Result<TodoItemDto>;

        // Assert
        await _repositoryMock.Received(1).GetByIdAsync(todoItem.Id);
        await _repositoryMock.Received(1).UpdateAsync(Arg.Is<TodoItem>(t => t.IsCompleted));
        await _unitOfWorkMock.Received(1).SaveChangesAsync();
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.True(result.Data.IsCompleted);
    }

    [Fact]
    public async Task Complete_TodoItemAlreadyCompleted_ReturnsSuccess()
    {
        // Arrange
        var todoItem = new TodoItem { Id = 1, Title = "Task", Description = "Description", IsCompleted = true, CreatedDate = DateTime.UtcNow };
        _repositoryMock.GetByIdAsync(todoItem.Id).Returns(todoItem);

        // Act
        var result = await _service.Complete(todoItem.Id) as Result<TodoItemDto>;

        // Assert
        await _repositoryMock.Received(1).GetByIdAsync(todoItem.Id);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.True(result.Data.IsCompleted);
    }

    [Fact]
    public async Task Complete_TodoItemNotFound_Failure()
    {
        // Arrange
        _repositoryMock.GetByIdAsync(Arg.Any<int>()).Returns((TodoItem)null);

        // Act
        var result = await _service.Complete(1);

        // Assert
        await _repositoryMock.Received(1).GetByIdAsync(1);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Contains("Todo Item Not Found", result.Errors.Select(e => e.Message));
    }
}