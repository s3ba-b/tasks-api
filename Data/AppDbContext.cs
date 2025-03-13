using Microsoft.EntityFrameworkCore;

namespace TasksApi.Models
{
    // TODO SB: Think about the name...
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; } = null!;
    }
}