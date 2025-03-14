namespace TasksApi.Models
{
    public class Task
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueTime { get; set; }
        public bool IsComplete { get; set; }
    }
}