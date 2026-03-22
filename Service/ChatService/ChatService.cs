public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository=chatRepository;
    }
    public async Task<Chat>CreateChatAsync(Chat chat)
    {
        return await _chatRepository.CreateAsync(chat);
    } 
}