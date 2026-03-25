using Microsoft.AspNetCore.SignalR;

public class MessageHub : Hub
{
    private readonly IMessageService _messageService;

    public MessageHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessage(long projectId, long senderId, string content)
    {
        var message = await _messageService.SaveMessage(senderId, projectId, content);

        await Clients.Group($"project-{projectId}")
            .SendAsync("ReceiveMessage", message);
    }

    public async Task JoinProject(long projectId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"project-{projectId}");
    }

    public async Task LeaveProject(long projectId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"project-{projectId}");
    }
}