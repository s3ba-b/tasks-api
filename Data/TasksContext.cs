using Microsoft.EntityFrameworkCore;

namespace TasksApi.Data
{
    // TODO SB: Think about the name...
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Task> Tasks { get; set; } = null!;
    }
}