using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<TodoItem> TodoItems { get; set; }
}

