public interface IChatService
{
    Task<Chat> CreateChatAsync(Chat chat);
}