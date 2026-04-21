using Microsoft.EntityFrameworkCore;

public class MessageService : IMessageService
{
    private readonly AppDbContext _context;

    public MessageService(AppDbContext context)
    {
        _context = context;
    }

    //Lưu message
  public async Task<Message> SaveMessage(long userId, long projectId, string content)
{
    var isMember = await _context.Projects
        .AnyAsync(p => p.Id == projectId && p.Team.Any(u => u.Id == userId));

    if (!isMember)
        throw new Exception("You are not in this project");

    var project = await _context.Projects
        .Include(p => p.Chat)
        .FirstOrDefaultAsync(p => p.Id == projectId);

    if (project == null)
        throw new Exception("Project not found");

    var chat = project.Chat;
    if (chat == null)
        throw new Exception("Chat not found");

    var message = new Message
    {
        SenderId = userId,
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
            .Include(m=>m.Sender)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
}