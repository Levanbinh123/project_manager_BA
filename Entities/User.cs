using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public long Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int ProjectSize { get; set; }

    public Role Role { get; set; }

    public List<Issue> AssignedIssues { get; set; } = new();
}
public enum Role
{
      ROLE_USER,ROLE_ADMIN
}