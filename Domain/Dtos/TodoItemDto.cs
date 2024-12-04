namespace Domain.Dtos;
public record TodoItemDto(int Id, string Title, string? Description, bool IsCompleted, DateTime CreatedDate);
