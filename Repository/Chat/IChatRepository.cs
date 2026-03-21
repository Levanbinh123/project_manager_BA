public interface IChatRepository 
{
    Task<List<Chat>> FindChatsByUserAsync(User user);
}