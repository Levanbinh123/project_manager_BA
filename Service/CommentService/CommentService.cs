public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IUserRepository _userRepository;

    public CommentService(
        ICommentRepository commentRepository,
        IIssueRepository issueRepository,
        IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _issueRepository = issueRepository;
        _userRepository = userRepository;
    }

    //  CREATE COMMENT
    public async Task<Comment> CreateCommentAsync(long issueId, long userId, string content)
    {
       var issue = await _issueRepository.GetByIdAsync(issueId);
        if (issue == null) throw new Exception("Issue not found");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        var comment = new Comment
        {
            IssueId = issue.Id,
            UserId = user.Id,
            Content = content,
            CreatedDateTime = DateTime.UtcNow
        };

        var savedComment = await _commentRepository.CreateAsync(comment);

        return savedComment;
    }


    public async Task DeleteCommentAsync(long commentId, long userId)
    {
        var comment = await _commentRepository.GetByIdAsync(commentId);
        if (comment == null) throw new Exception("Comment not found");

        if (comment.UserId != userId)
        {
            throw new Exception("You are not allowed to delete this comment");
        }

        await _commentRepository.DeleteAsync(commentId);
    }

    // 🔥 GET COMMENTS BY ISSUE
    public async Task<List<Comment>> FindCommentsByIssueIdAsync(long issueId)
    {
        return await _commentRepository.FindByIssueIdAsync(issueId);
    }
}