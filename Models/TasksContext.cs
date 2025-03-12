using Microsoft.EntityFrameworkCore;

namespace TasksApi.Models;

public class TaskItemsContext : DbContext
{
    public TaskItemsContext(DbContextOptions<TaskItemsContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; } = null!;
}