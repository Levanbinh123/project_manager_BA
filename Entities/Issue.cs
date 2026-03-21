using System.ComponentModel.DataAnnotations.Schema;

public class Issue
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Status { get; set; }

    public long ProjectId { get; set; }
    public Project Project { get; set; }

    public string Priority { get; set; }

    public DateTime? DueDate { get; set; }
    [Column(TypeName = "json")]
public List<string> Tags { get; set; } = new();

    public long? AssigneeId { get; set; }
    public User Assignee { get; set; }

    public List<Comment> Comments { get; set; } = new();
}