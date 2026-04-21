public class MessageDTO
{
    public long Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public long? SenderId { get; set; } 
}