public class TaskQueryParameters
{

    public DateTime? MinDueTime { get; set; }
    public DateTime? MaxDueTime { get; set; }
    public bool? IsComplete { get; set; }

    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}