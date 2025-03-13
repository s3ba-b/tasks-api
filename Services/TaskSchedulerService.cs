using Microsoft.EntityFrameworkCore;
using TasksApi.Models;

namespace TasksApi.Services
{
    public class TaskSchedulerService : ITaskSchedulerService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ITaskSchedulerService> _logger;

        public TaskSchedulerService(AppDbContext context, ILogger<TaskSchedulerService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task CheckDueTasksAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();

            DateTime threshold = DateTime.UtcNow.AddHours(2);

            var dueTasks = tasks.Where(task => task.DueTime <= threshold).ToList();

            if (dueTasks.Any())
            {
                foreach (var task in dueTasks)
                {
                    _logger.LogWarning($"Task '{task.Title}' (ID: {task.Id}) is due at {task.DueTime}.");
                }
            }
            else
            {
                _logger.LogInformation("No tasks are due within the next 2 hours.");
            }
        }
    }
}