using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.EntityFrameworkCore;
using TasksApi.Models;
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
        Cron.Minutely);
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
            new TasksApi.Models.Task { Id = 10, Title = "Dust the furniture", Description = "Dust the furniture and clean the surfaces", DueTime = DateTime.UtcNow.AddHours(3), IsComplete = false }
        );
        context.SaveChanges();
    }
}
