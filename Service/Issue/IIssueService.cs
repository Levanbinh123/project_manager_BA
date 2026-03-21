public interface IIssueService
{
    Task<Issue> GetIssueById(int issueId);
    Task<List<Issue>> GetIssuesByProjectId(int projectId);
    Task<Issue> CreateIssue(IssueRequest request, User user);
    Task DeleteIssue(int issueId, int userId);
    Task<Issue> AddUserToIssue(int issueId, int userId);
    Task<Issue> UpdateStatus(int issueId, string status);
}