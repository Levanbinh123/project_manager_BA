using System.ComponentModel.DataAnnotations.Schema;

public class Project
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Category { get; set; }
    [Column(TypeName = "json")]
        public List<string> Tags { get; set; } = new();

    public Chat Chat { get; set; }

    public long? OwnerId { get; set; }
    public User Owner { get; set; }

    public List<Issue> Issues { get; set; } = new();

    public List<User> Team { get; set; } = new();
}