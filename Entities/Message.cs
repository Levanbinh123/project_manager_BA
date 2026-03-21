public class Message
{
    public long Id{get; set;}
    public string Content{get;set;}
    public DateTime CreatedAt{get;set;}
    public long ChatId{get;set;}
    public Chat chat{get;set;}
    public long? SenderId{get;set;}
    public User Sender{get;set;}
}