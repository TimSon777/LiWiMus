using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Roles;
using Microsoft.AspNetCore.SignalR;

namespace LiWiMus.Web.Hubs.SupportChat;

public partial class SupportChatHub
{
    public async Task ConnectConsultant()
    {
        var userClaims = Context.User;
        
        if (userClaims?.IsInRole(DefaultRoles.Consultant.Name) == false)
        {
            return;
        }

        var user = await _userManager.GetUserAsync(userClaims);

        var oldCons = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantByUser(user));

        if (oldCons is null)
        {
            var consultant = new OnlineConsultant
            {
                Consultant = user,
                ConnectionId = Context.ConnectionId
            };

        
            await _onlineConsultantsRepository.AddAsync(consultant);
            await _onlineConsultantsRepository.SaveChangesAsync();
        }
    }
    
    public async Task SendMessageToUser(string connectionId, string text)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        
        var onlineConsultant = await _onlineConsultantsRepository
            .GetBySpecAsync(new OnlineConsultantByConnectionIdSpec(Context.ConnectionId));
        
        var chat = onlineConsultant?.Chats.FirstOrDefault(c => c.UserConnectionId == connectionId);
        
        if (chat is null)
        {
            return;
        }

        await _messageRepository.AddAsync(new Message
        {
            Chat = chat,
            Text = text,
            Sender = user
        });
        
        await _messageRepository.SaveChangesAsync();

        await Clients.Group(chat.User.UserName).SendAsync("SendMessageToUser", text);
    }
        
    public async Task DisconnectConsultant()
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantByUser(user));
        
        if (consultant is null)
        {
            return;
        }
        
        var chats = consultant.Chats.Where(c => c.Status == ChatStatus.Opened);
        
        await _onlineConsultantsRepository.DeleteAsync(consultant);
        
        foreach (var chat in chats)
        {
            var newConsultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantWithMinimalWorkload(consultant));

            if (newConsultant is null)
            {
                return;
            }

            chat.ConsultantConnectionId = newConsultant.ConnectionId;
        }
    } 
}
