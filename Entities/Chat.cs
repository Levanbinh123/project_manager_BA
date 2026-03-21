using System.ComponentModel.DataAnnotations;

public class Chat
{
    [Key]
    public long Id{get; set;}
    public string name{get;set;}
    public long? ProjectId{get;set;}
     public Project Project { get; set; }
    public List<Message>Messages{get;set;}=new();
    public List<User> Users{get;set;}=new();
}