using Microsoft.AspNetCore.SignalR;

namespace LiWiMus.Web.Hubs;

public class SupportChatConsultantHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Consultants");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Consultants");
        await base.OnDisconnectedAsync(exception);
    }
}