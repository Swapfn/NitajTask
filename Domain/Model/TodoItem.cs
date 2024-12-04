namespace Domain.Model;
public class TodoItem
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
