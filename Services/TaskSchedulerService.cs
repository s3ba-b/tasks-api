using Microsoft.EntityFrameworkCore;
using TasksApi.Data;
using TasksApi.Models;
using TasksApi.Repositories;

namespace TasksApi.Services
{
    public class TaskSchedulerService : ITaskSchedulerService
    {
        private readonly IRepository<Models.Task> _repository;
        private readonly ILogger<ITaskSchedulerService> _logger;

        public TaskSchedulerService(IRepository<Models.Task> repository, ILogger<TaskSchedulerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async System.Threading.Tasks.Task CheckDueTasksAsync()
        {
            var tasks = await _repository.GetAllAsync();

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