namespace TasksApi.Services
{
    public interface ITaskSchedulerService
    {
        Task CheckDueTasksAsync();
    }
}