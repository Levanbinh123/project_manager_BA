using Microsoft.EntityFrameworkCore;

public class MessageRepository: IMessageRepository
{
    private readonly AppDbContext _context;
    public MessageRepository(AppDbContext context)
    {
        _context=context;
    }
    public async Task<List<Message>> FindByChatIdOrderByCreatedAtAscAsync(long chatId)
    {
        return await _context.Messages
        .Where(m=>m.ChatId==chatId)
        .OrderBy(m=>m.CreatedAt)
        .Include(m=>m.Sender).ToListAsync();
    }
}