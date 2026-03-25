using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("api/comments")]
[Authorize]
public class CommentContorller : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IUserService _userService;
        public CommentContorller(ICommentService commentService, IUserService userService)
    {
        _commentService = commentService;
        _userService = userService;
    }
    [HttpPost]
    public async Task<ActionResult<CommentDTO>>CreateComment([FromBody] CreateCommentRequest request,[FromHeader(Name ="Authorization")] string authorization)
    {
        var user=await _userService.FindUserProfileByJwt(authorization);
        var comment=await _commentService.CreateCommentAsync(request.IssueId,user.Id,request.Content);
        var dto= new CommentDTO
        {
            Id=comment.Id,
            Content=comment.Content,
            User=user
        };
         return CreatedAtAction(nameof(GetCommentsByIssueId), new { issueId = request.IssueId }, dto);
    }
    [HttpDelete("{commentId}")]
    public async Task<ActionResult<MessageResponse>>DeleteComment(
        long commentId,[FromHeader(Name ="Authorization")] string authorization
    )
    {
        var user=await _userService.FindUserProfileByJwt(authorization);
        await _commentService.DeleteCommentAsync(commentId,user.Id);
        return Ok(new MessageResponse { Message = "Comment deleted" });
    }
    [HttpGet("{issueId}")]
    public async Task<ActionResult<List<CommentDTO>>>GetCommentsByIssueId(long issueId)
    {
        var comments=await _commentService.FindCommentsByIssueIdAsync(issueId);
        var dtos=comments.Select(c=>new CommentDTO
        {
            Id=c.Id,
            Content=c.Content,
            User=c.User
        }).ToList();
        return Ok(dtos);
    }
}