using Microsoft.EntityFrameworkCore;

public class IssueRepository : IIssueRepository
{
    private readonly AppDbContext _context;

    public IssueRepository(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 findByProjectId
    public async Task<List<Issue>> FindByProjectIdAsync(long projectId)
    {
        return await _context.Issues
            .Where(i => i.ProjectId == projectId)
            .Include(i => i.Assignee)
            .Include(i => i.Project)
            .ToListAsync();
    }

    // 🔹 findByAssignee
    public async Task<List<Issue>> FindByAssigneeAsync(User assignee)
    {
        return await _context.Issues
            .Where(i => i.AssigneeId == assignee.Id)
            .Include(i => i.Project)
            .ToListAsync();
    }
    public async Task<Issue?>GetByIdAsync(long id)
    {
        return await _context.Issues
        .Include(i=>i.Assignee)
        .Include(i=>i.Project)
        .FirstOrDefaultAsync(i=>i.Id==id);
    }
    public async Task<Issue>CreateAsync(Issue issue)
    {
        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();
        return issue;
    }
}