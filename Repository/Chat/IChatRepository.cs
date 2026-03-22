public interface IChatRepository 
{   
        Task<Chat> CreateAsync(Chat chat);
    Task<List<Chat>> FindChatsByUserAsync(User user);
}