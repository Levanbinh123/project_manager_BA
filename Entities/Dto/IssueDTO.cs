public class IssueDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime? DueDate { get; set; }

    public long ProjectId { get; set; }
    public string? ProjectName { get; set; }

    public long? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }

    public List<string>? Tags { get; set; }
}