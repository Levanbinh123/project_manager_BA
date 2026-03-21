using Microsoft.EntityFrameworkCore;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;
    public CommentRepository(AppDbContext context)
    {
        _context=context;
    }
    public async Task<List<Comment>> FindByIssueIdAsync(long issueId)
    {
        return await _context.Comments
        .Where(c=>c.IssueId==issueId)
        .Include(c=>c.User)
        .ToListAsync();
    }
       public async Task<Comment?> GetByIdAsync(long id)
    {
        return await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Issue)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task<bool> DeleteAsync(long id)
    {
        var comment=await _context.Comments.FindAsync(id);
        if(comment==null)return false;
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Comment> CreateAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
}