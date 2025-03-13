using Microsoft.EntityFrameworkCore;
using TasksApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("Tasks"));

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData(dbContext);
}

// Configure the HTTP request pipeline.
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

app.Run();

// Seed Data Method
void SeedData(AppDbContext context)
{
    if (!context.Tasks.Any()) // Prevent duplicate seeding
    {
        context.Tasks.AddRange(
            new TasksApi.Models.Task { Id = 1, Title = "Buy groceries", Description = "Milk, Cheese, Pizza, Fruit, Tylenol", DueTime = DateTime.Now.AddDays(1), IsComplete = false },
            new TasksApi.Models.Task { Id = 2, Title = "Clean room", Description = "Clean the room and make the bed", DueTime = DateTime.Now.AddDays(2), IsComplete = false },
            new TasksApi.Models.Task { Id = 3, Title = "Wash car", Description = "Wash the car and clean the interior", DueTime = DateTime.Now.AddDays(3), IsComplete = false },
            new TasksApi.Models.Task { Id = 4, Title = "Do laundry", Description = "Wash, dry, and fold the laundry", DueTime = DateTime.Now.AddDays(4), IsComplete = false },
            new TasksApi.Models.Task { Id = 5, Title = "Mow the lawn", Description = "Mow the front and back lawn", DueTime = DateTime.Now.AddDays(5), IsComplete = false },
            new TasksApi.Models.Task { Id = 6, Title = "Take out the trash", Description = "Take out the trash and recycling", DueTime = DateTime.Now.AddDays(6), IsComplete = false },
            new TasksApi.Models.Task { Id = 7, Title = "Walk the dog", Description = "Take the dog for a walk", DueTime = DateTime.Now.AddDays(7), IsComplete = false },
            new TasksApi.Models.Task { Id = 8, Title = "Water the plants", Description = "Water the indoor and outdoor plants", DueTime = DateTime.Now.AddDays(8), IsComplete = false },
            new TasksApi.Models.Task { Id = 9, Title = "Vacuum the house", Description = "Vacuum the floors and clean the carpets", DueTime = DateTime.Now.AddDays(9), IsComplete = false },
            new TasksApi.Models.Task { Id = 10, Title = "Dust the furniture", Description = "Dust the furniture and clean the surfaces", DueTime = DateTime.Now.AddDays(10), IsComplete = false }
        );
        context.SaveChanges();
    }
}
