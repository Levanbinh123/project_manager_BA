public interface ICommentRepository
{
    Task<List<Comment>> FindByIssueIdAsync(long issueId);
     Task<Comment?> GetByIdAsync(long id);
    Task<Comment> CreateAsync(Comment comment);
    Task<bool> DeleteAsync(long id);
}