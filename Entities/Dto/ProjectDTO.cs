public class ProjectDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string>? Tags { get; set; } = new();
    public string? Category { get; set; }
    public User? Owner { get; set; }
    public List<User>? Team { get; set; } = new();
    public Chat? Chat { get; set; }
}