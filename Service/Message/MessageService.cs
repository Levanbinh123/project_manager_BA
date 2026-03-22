using Microsoft.EntityFrameworkCore;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;

    public MessageService(AppDbContext context)
    {
        _context = context;
    }

    //Lưu message
    public async Task<Message> SaveMessage(long senderId, long projectId, string content)
    {
        // Lấy user
        var user = await _context.Users.FindAsync(senderId);
        if (user == null)
            throw new Exception("User not found");

        // Lấy project + chat
        var project = await _context.Projects
            .Include(p => p.Chat)
            .ThenInclude(c => c.Messages)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            throw new Exception("Project not found");

        var chat = project.Chat;
        if (chat == null)
            throw new Exception("Chat not found");

        // Tạo message
        var message = new Message
        {
            Sender = user,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            ChatId = chat.Id
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return message;
    }

    // Lấy message theo project
    public async Task<List<Message>> GetMessagesByProjectId(long projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Chat)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
            throw new Exception("Project not found");

        var chat = project.Chat;
        if (chat == null)
            throw new Exception("Chat not found");

        return await _context.Messages
            .Where(m => m.ChatId == chat.Id)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
}