using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using TasksApi.Data;
using TasksApi.Repositories;
using TasksApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("Tasks"));

builder.Services.AddHangfire(config =>
    config.UseMemoryStorage());
builder.Services.AddHangfireServer();

builder.Services.AddTransient<ITaskSchedulerService, TaskSchedulerService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    var taskChecker = scope.ServiceProvider.GetRequiredService<ITaskSchedulerService>();
    recurringJobManager.AddOrUpdate(
        "my-job",
        () => taskChecker.CheckDueTasksAsync(),
        Cron.Hourly);
}

app.Run();

void SeedData(AppDbContext context)
{
    if (!context.Tasks.Any())
    {
        context.Tasks.AddRange(
            new TasksApi.Models.Task { Id = 1, Title = "Buy groceries", Description = "Milk, Cheese, Pizza, Fruit, Tylenol", DueTime = DateTime.UtcNow.AddHours(1), IsComplete = false },
            new TasksApi.Models.Task { Id = 2, Title = "Clean room", Description = "Clean the room and make the bed", DueTime = DateTime.UtcNow.AddHours(2), IsComplete = false },
            new TasksApi.Models.Task { Id = 3, Title = "Wash car", Description = "Wash the car and clean the interior", DueTime = DateTime.UtcNow.AddHours(3), IsComplete = false },
            new TasksApi.Models.Task { Id = 4, Title = "Do laundry", Description = "Wash, dry, and fold the laundry", DueTime = DateTime.UtcNow.AddHours(4), IsComplete = false },
            new TasksApi.Models.Task { Id = 5, Title = "Mow the lawn", Description = "Mow the front and back lawn", DueTime = DateTime.UtcNow.AddDays(1), IsComplete = false },
            new TasksApi.Models.Task { Id = 6, Title = "Take out the trash", Description = "Take out the trash and recycling", DueTime = DateTime.UtcNow.AddDays(2), IsComplete = false },
            new TasksApi.Models.Task { Id = 7, Title = "Walk the dog", Description = "Take the dog for a walk", DueTime = DateTime.UtcNow.AddDays(3), IsComplete = false },
            new TasksApi.Models.Task { Id = 8, Title = "Water the plants", Description = "Water the indoor and outdoor plants", DueTime = DateTime.UtcNow.AddDays(4), IsComplete = false },
            new TasksApi.Models.Task { Id = 9, Title = "Vacuum the house", Description = "Vacuum the floors and clean the carpets", DueTime = DateTime.UtcNow.AddDays(5), IsComplete = false },
            new TasksApi.Models.Task { Id = 10, Title = "Dust the furniture", Description = "Dust the furniture and clean the surfaces", DueTime = DateTime.UtcNow.AddHours(3), IsComplete = false },
            new TasksApi.Models.Task { Id = 11, Title = "Read a book", Description = "Read the new novel", DueTime = DateTime.UtcNow.AddDays(6), IsComplete = false },
            new TasksApi.Models.Task { Id = 12, Title = "Write a report", Description = "Write the monthly report", DueTime = DateTime.UtcNow.AddDays(7), IsComplete = false },
            new TasksApi.Models.Task { Id = 13, Title = "Exercise", Description = "Go for a run", DueTime = DateTime.UtcNow.AddDays(8), IsComplete = false },
            new TasksApi.Models.Task { Id = 14, Title = "Cook dinner", Description = "Prepare a healthy meal", DueTime = DateTime.UtcNow.AddDays(9), IsComplete = false },
            new TasksApi.Models.Task { Id = 15, Title = "Call mom", Description = "Catch up with mom", DueTime = DateTime.UtcNow.AddDays(10), IsComplete = false },
            new TasksApi.Models.Task { Id = 16, Title = "Pay bills", Description = "Pay the monthly bills", DueTime = DateTime.UtcNow.AddDays(11), IsComplete = false },
            new TasksApi.Models.Task { Id = 17, Title = "Plan vacation", Description = "Plan the summer vacation", DueTime = DateTime.UtcNow.AddDays(12), IsComplete = false },
            new TasksApi.Models.Task { Id = 18, Title = "Organize closet", Description = "Organize the closet and donate old clothes", DueTime = DateTime.UtcNow.AddDays(13), IsComplete = false },
            new TasksApi.Models.Task { Id = 19, Title = "Fix the sink", Description = "Fix the leaky sink", DueTime = DateTime.UtcNow.AddDays(14), IsComplete = false },
            new TasksApi.Models.Task { Id = 20, Title = "Attend meeting", Description = "Attend the team meeting", DueTime = DateTime.UtcNow.AddDays(15), IsComplete = false },
            new TasksApi.Models.Task { Id = 21, Title = "Write blog post", Description = "Write a new blog post", DueTime = DateTime.UtcNow.AddDays(16), IsComplete = false },
            new TasksApi.Models.Task { Id = 22, Title = "Meditate", Description = "Meditate for 20 minutes", DueTime = DateTime.UtcNow.AddDays(17), IsComplete = false },
            new TasksApi.Models.Task { Id = 23, Title = "Grocery shopping", Description = "Buy groceries for the week", DueTime = DateTime.UtcNow.AddDays(18), IsComplete = false },
            new TasksApi.Models.Task { Id = 24, Title = "Clean garage", Description = "Clean and organize the garage", DueTime = DateTime.UtcNow.AddDays(19), IsComplete = false },
            new TasksApi.Models.Task { Id = 25, Title = "Car maintenance", Description = "Take the car for maintenance", DueTime = DateTime.UtcNow.AddDays(20), IsComplete = false },
            new TasksApi.Models.Task { Id = 26, Title = "Study", Description = "Study for the upcoming exam", DueTime = DateTime.UtcNow.AddDays(21), IsComplete = false },
            new TasksApi.Models.Task { Id = 27, Title = "Practice guitar", Description = "Practice guitar for 30 minutes", DueTime = DateTime.UtcNow.AddDays(22), IsComplete = false },
            new TasksApi.Models.Task { Id = 28, Title = "Bake cookies", Description = "Bake chocolate chip cookies", DueTime = DateTime.UtcNow.AddDays(23), IsComplete = false },
            new TasksApi.Models.Task { Id = 29, Title = "Watch movie", Description = "Watch a new movie", DueTime = DateTime.UtcNow.AddDays(24), IsComplete = false },
            new TasksApi.Models.Task { Id = 30, Title = "Call friend", Description = "Catch up with an old friend", DueTime = DateTime.UtcNow.AddDays(25), IsComplete = false }
        );
        context.SaveChanges();
    }
}
