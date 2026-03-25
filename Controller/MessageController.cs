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
    public async Task<ActionResult<SendMessageResponse>> SendMessages(
        [FromBody] CreateMessageRequest req)
    {
        var user = await _userService.FindUserById(req.SenderId);
        if (user == null) return BadRequest("User not found");

        var message = await _messageService.SaveMessage(req.SenderId, req.ProjectId, req.Content);

        var res = new SendMessageResponse
        {
            Id = message.Id,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };

        return Ok(res);
    }

    // 🔹 Get messages
    [HttpGet("chat/{projectId}")]
    public async Task<ActionResult<List<Message>>> GetMessagesByProjectId(long projectId)
    {
        var messages = await _messageService.GetMessagesByProjectId(projectId);
        return Ok(messages);
    }
}