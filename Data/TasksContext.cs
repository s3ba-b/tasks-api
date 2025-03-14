using Microsoft.EntityFrameworkCore;

namespace TasksApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Task> Tasks { get; set; } = null!;
    }
}