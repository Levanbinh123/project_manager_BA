public class Comment
{
    public long Id { get; set; }

    public string Content { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public long UserId { get; set; }
    public User User { get; set; }

    public long IssueId { get; set; }
    public Issue Issue { get; set; }
}