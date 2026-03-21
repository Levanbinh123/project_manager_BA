using Microsoft.EntityFrameworkCore;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;
    public ChatRepository(AppDbContext context)
    {
        _context=context;
    }
    public async Task<List<Chat>>  FindChatsByUserAsync(User user)
    {
        return await _context.Chats.Where(c=>c.Users.Any(u=>u.Id==user.Id))
        .Include(c=>c.Users)
        .Include(c=>c.Messages)
        .ToListAsync();
    }
    public async Task<Chat> CreateAsync(Chat chat)
    {
        _context.Chats.Add(chat);
        await _context.SaveChangesAsync();
        return chat;
    }
}