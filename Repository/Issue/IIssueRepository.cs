public interface IIssueRepository
{
    Task<List<Issue>> FindByProjectIdAsync(long projectId);
    Task<List<Issue>> FindByAssigneeAsync(User assignee);
      Task<Issue?> GetByIdAsync(long id);
        Task<Issue> CreateAsync(Issue issue);
}