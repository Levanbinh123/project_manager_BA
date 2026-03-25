using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/issues")]
[Authorize]
public class IssueController : ControllerBase
{
    private readonly IIssueService _issueService;
    private readonly IUserService _userService;

    public IssueController(IIssueService issueService, IUserService userService)
    {
        _issueService = issueService;
        _userService = userService;
    }


    [HttpGet("{issueId}")]
    public async Task<ActionResult<IssueDTO>> GetIssueById(long issueId)
    {
        var issue = await _issueService.GetIssueById(issueId);
        if (issue == null) return NotFound("Issue not found");

        var dto = new IssueDTO
        {
            Id = issue.Id,
            Title = issue.Title,
            Description = issue.Description,
            Status = issue.Status,
            Priority = issue.Priority,
            DueDate = issue.DueDate,
            ProjectId = issue.Project.Id,
            ProjectName = issue.Project.Name,
            AssigneeId = issue.Assignee?.Id,
            AssigneeName = issue.Assignee?.FullName,
            Tags = issue.Tags
        };

        return Ok(dto);
    }


    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<List<IssueDTO>>> GetIssuesByProjectId(long projectId)
    {
        var issues = await _issueService.GetIssuesByProjectId(projectId);

        var dtos = issues.Select(i => new IssueDTO
        {
            Id = i.Id,
            Title = i.Title,
            Description = i.Description,
            Status = i.Status,
            Priority = i.Priority,
            DueDate = i.DueDate,
            ProjectId = i.Project.Id,
            ProjectName = i.Project.Name
        }).ToList();

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<IssueDTO>> CreateIssue(
        [FromBody] IssueRequest request,
        [FromHeader(Name = "Authorization")] string authorization)
    {
        var tokenUser = await _userService.FindUserProfileByJwt(authorization);
        var user = await _userService.FindUserById(tokenUser.Id);

        var issue = await _issueService.CreateIssue(request, user);

        var dto = new IssueDTO
        {
            Id = issue.Id,
            Title = issue.Title,
            Description = issue.Description,
            Status = issue.Status,
            Priority = issue.Priority,
            DueDate = issue.DueDate,
            ProjectId = issue.Project.Id,
            ProjectName = issue.Project.Name
        };

        return StatusCode(201, dto);
    }

    [HttpDelete("{issueId}")]
    public async Task<ActionResult> DeleteIssue(
        long issueId,
        [FromHeader(Name = "Authorization")] string authorization)
    {
        var user = await _userService.FindUserProfileByJwt(authorization);
        await _issueService.DeleteIssue(issueId, user.Id);

        return Ok(new { Message = "Successfully deleted" });
    }

    [HttpPut("{issueId}/assignee/{userId}")]
    public async Task<ActionResult> AddUserToIssue(long issueId, long userId)
    {
        await _issueService.AddUserToIssue(issueId, userId);
        return Ok(new { Message = "Successfully added" });
    }

    [HttpPut("{issueId}/status/{status}")]
    public async Task<ActionResult> UpdateIssueStatus(long issueId, string status)
    {
        await _issueService.UpdateStatus(issueId, status);
        return Ok(new { Message = "Successfully updated" });
    }
}