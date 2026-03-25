public interface IIssueService
{
    Task<Issue> GetIssueById(long issueId);
    Task<List<Issue>> GetIssuesByProjectId(long projectId);
    Task<Issue> CreateIssue(IssueRequest request, User user);
    Task DeleteIssue(long issueId, long userId);
    Task<Issue> AddUserToIssue(long issueId, long userId);
    Task<Issue> UpdateStatus(long issueId, string status);
}