using Microsoft.EntityFrameworkCore;

public class IssueService : IIssueService
{
    private readonly AppDbContext _context;

    public IssueService(AppDbContext context)
    {
        _context = context;
    }

    //  Get Issue by Id
    public async Task<Issue> GetIssueById(int issueId)
    {
        var issue = await _context.Issues
            .Include(i => i.Project)
            .Include(i => i.Assignee)
            .FirstOrDefaultAsync(i => i.Id == issueId);

        if (issue == null)
            throw new Exception($"Issue not found {issueId}");

        return issue;
    }

    // Get Issues by Project
    public async Task<List<Issue>> GetIssuesByProjectId(int projectId)
    {
        return await _context.Issues
            .Where(i => i.ProjectId == projectId)
            .ToListAsync();
    }

    // Create Issue
    public async Task<Issue> CreateIssue(IssueRequest request, User user)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId);

        if (project == null)
            throw new Exception("Project not found");

        var issue = new Issue
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            Project = project
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        return issue;
    }

    //  Delete Issue
    public async Task DeleteIssue(int issueId, int userId)
    {
        var issue = await GetIssueById(issueId);

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync();
    }

    //  Assign User to Issue
    public async Task<Issue> AddUserToIssue(int issueId, int userId)
    {
        var issue = await _context.Issues
            .FirstOrDefaultAsync(i => i.Id == issueId);

        if (issue == null)
            throw new Exception("Issue not found");

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        issue.Assignee = user;

        await _context.SaveChangesAsync();

        return issue;
    }

    // Update Status
    public async Task<Issue> UpdateStatus(int issueId, string status)
    {
        var issue = await GetIssueById(issueId);

        issue.Status = status;

        await _context.SaveChangesAsync();

        return issue;
    }
}