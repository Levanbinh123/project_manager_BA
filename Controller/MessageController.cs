using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/message")]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public MessageController(IMessageService messageService, IUserService userService)
    {
        _messageService = messageService;
        _userService = userService;
    }

    // 🔹 Send message (REST)
    [HttpPost("send")]
    public async Task<ActionResult<MessageDTO>> SendMessages(
        [FromBody] CreateMessageRequest req,
    [FromHeader(Name = "Authorization")] string authorization)
    {
        var user=await _userService.FindUserProfileByJwt(authorization);
        var message=await _messageService.SaveMessage(
            user.Id,
            req.ProjectId,
            req.Content
        );
        var dto=new MessageDTO
        {
            Id=message.Id,
            Content=message.Content,
            CreatedAt=message.CreatedAt,
            SenderId=message.SenderId
        };
        return Ok(dto);
    }

    // 🔹 Get messages
    [HttpGet("chat/{projectId}")]
    public async Task<ActionResult<List<Message>>> GetMessagesByProjectId(long projectId)
    {
        var messages = await _messageService.GetMessagesByProjectId(projectId);
        var dtos=messages.Select(m=>new MessageDTO
        {
            Id=m.Id,
            Content=m.Content,
            CreatedAt=m.CreatedAt,
            SenderId=m.SenderId
        }).ToList();
        return Ok(dtos);
    }
}