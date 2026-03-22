public interface IMessageService
{
    Task<Message> SaveMessage(long senderId, long projectId, string content);
    Task<List<Message>> GetMessagesByProjectId(long projectId);
}