using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
[ApiController]
[Route("/api/projects")]
[Authorize]
public class ProjectController  : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IUserService _userSerice;
    private readonly IInvitationService _invitationService;
    public ProjectController(
        IProjectService projectService,
        IUserService userService,
        IInvitationService invitationService
    )
    {
        _projectService=projectService;
        _userSerice=userService;
        _invitationService=invitationService;
    }
    private async Task<User> GetCurrentUser()
    {
        var email=User.Identity.Name;
        return await _userSerice.FindUserByEmail(email);
    }
    [HttpGet]
    public async Task<ActionResult<List<Project>>> GetProjects(
        [FromQuery] string? category,
        [FromQuery] string? tag
    )
    {
        var user=await GetCurrentUser();
        var projects=await _projectService.GetProjectByTeam(user, category, tag);
        return Ok(projects);
    }
    [HttpGet("{projectId}")]
    public async Task<ActionResult<Project>> GetProjectById(long projectId)
    {
        var project=await _projectService.GetProjectById(projectId);
        return Ok(project);
    }
    [HttpPost]
    public async Task<ActionResult> CreateProject([FromBody] ProjectDTO project)
    {
        var user=await GetCurrentUser();
        await _projectService.CreateProject(project, user);
        return Ok("Da tao du an thanh cong" );

    }
    [HttpPatch("{projectId}")]
    public async Task<ActionResult> UpdateProject(long projectId,[FromBody] UpdateProject project)
    {
        await _projectService.UpdateProject(projectId, project);
        return Ok("Cap nhat project");
    }
    [HttpDelete("{projectId}")]
    public async Task<ActionResult>DeleteProject(long projectId)
    {
        var user=await GetCurrentUser();
        await _projectService.DeleteProject(projectId,user.Id);
        return Ok("Xoa thanh cong");
    }
    [HttpGet("search")]
    public async Task<ActionResult<List<ProjectDTO>>>SearchProjects(
        [FromQuery]string? keyword
    )
    {
        var user=await GetCurrentUser();
        var projects=await _projectService.SearchProjects(keyword, user);
        return Ok(projects);
    }
    [HttpGet("{projectId}/chat")]
    public async Task<ActionResult<Chat>>GetChatByProjectId(long projectId)
    {
        var chat=await _projectService.GetChatByProjectId(projectId);
        return Ok(chat);
    }
    [HttpPost("invite")]
    public async Task<ActionResult>InviteProject([FromBody] InviteRequest req)
    {
        await _invitationService.SendInvitation(req.Email, req.ProjectId);
        return Ok("User invited");
    }
    [HttpGet("accept_invitation")]
    public async Task<ActionResult<Invitation>> AcceptInvitation([FromQuery] string token)
    {
        var user = await GetCurrentUser();

        var invitation = await _invitationService.AcceptInvitation(token, user.Id);

        await _projectService.AddUserToProject(invitation.ProjectId, user.Id);

        return StatusCode(202, invitation);
    }
}
