public interface IMessageService
{
    Task<Message> SaveMessage(int senderId, int projectId, string content);
    Task<List<Message>> GetMessagesByProjectId(int projectId);
}