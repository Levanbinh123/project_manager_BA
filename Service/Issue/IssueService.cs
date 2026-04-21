using Microsoft.EntityFrameworkCore;

public class IssueService : IIssueService
{
    private readonly AppDbContext _context;

    public IssueService(AppDbContext context)
    {
        _context = context;
    }

    //  Get Issue by Id
    public async Task<Issue> GetIssueById(long issueId)
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
    public async Task<List<Issue>> GetIssuesByProjectId(long projectId)
    {
        return await _context.Issues
        .Include(i=>i.Project)
        .Include(i=>i.Assignee)
            .Where(i => i.ProjectId == projectId)
            .ToListAsync();
    }

    // Create Issue
    public async Task<Issue> CreateIssue(IssueRequest request, User user)
    {
        
        
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId);
        if (project.Owner.Id != user.Id)
        {
            throw new Exception("Only leader can create task");
        }

        if (project == null)
            throw new Exception("Project not found");
               User? assignee = null;

        //  nếu có truyền AssigneeId thì tìm user
        if (request.AssigneeId.HasValue)
        {
            assignee = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.AssigneeId.Value);

            if (assignee == null)
                throw new Exception("Assignee not found");
        }

        var issue = new Issue
        {
            Title = request.Title,
            Description = request.Description,
            Status = request?.Status,
            Priority = request?.Priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId,
            Project = project,
            Assignee=assignee
        };

        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();

        return issue;
    }

    //  Delete Issue
    public async Task DeleteIssue(long issueId, long userId)
    {
        var issue = await GetIssueById(issueId);

        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync();
    }

    //  Assign User to Issue
    public async Task<Issue> AddUserToIssue(long issueId, long userId)
    {
        var issue = await _context.Issues
        .Include(i=>i.Assignee)
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
    public async Task<Issue> UpdateStatus(long issueId, string status)
    {
        var issue = await GetIssueById(issueId);

        issue.Status = status;

        await _context.SaveChangesAsync();

        return issue;
    }
}