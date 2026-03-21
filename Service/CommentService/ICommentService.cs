public interface ICommentService
{
    Task<Comment> CreateCommentAsync(long issueId, long userId, string content);
    Task DeleteCommentAsync(long commentId, long userId);
    Task<List<Comment>> FindCommentsByIssueIdAsync(long issueId);
}