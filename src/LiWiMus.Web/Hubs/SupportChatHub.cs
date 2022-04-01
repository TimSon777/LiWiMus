using LiWiMus.Core.Constants;
using LiWiMus.Core.Entities;
using LiWiMus.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace LiWiMus.Web.Hubs;

public class SupportChatHub : Hub
{
    private readonly ApplicationContext _dbContext;
    private readonly UserManager<User> _userManager;

    public SupportChatHub(ApplicationContext dbContext, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }
    
    public override async Task OnConnectedAsync()
    {
    }
    
    public async Task SendMessageToUser(string sender, string receiver, string message)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        await Clients.Group(user.UserName).SendAsync("ReceiveMessage", sender, message);
    }
}