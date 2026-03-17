namespace RestAPI.Models;

using Microsoft.EntityFrameworkCore;

public class TodoContext(DbContextOptions<TodoContext> options) : DbContext(options)
{
    public DbSet<TodoItem> TodoItems { get; set; } = null!;
}