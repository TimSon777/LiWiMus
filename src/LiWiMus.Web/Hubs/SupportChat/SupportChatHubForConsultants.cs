using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Permissions;
using LiWiMus.Core.Roles;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> CloseChatByConsultant(string userName)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantByUser(user));
        var chat = consultant!.Chats.FirstOrDefault(c => c.User.UserName == userName);
        
        if (chat is null)
        {
            return new BadRequestResult();
        }
        
        if (chat.Status != ChatStatus.Opened)
        {
            return new BadRequestResult();
        }

        chat.Status = ChatStatus.ClosedByConsultant;
        
        await _repositoryChat.SaveChangesAsync();

        await SendMessageToUser(chat.UserConnectionId, "Chat was closed by cons");
        return new OkResult();
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
            var chatVm = _mapper.Map<ChatViewModel>(chat);

            chat.ConsultantConnectionId = newConsultant.ConnectionId;
            newConsultant.Chats.Add(chat);
            
            var chatForConsultant = await _razorViewRenderer.RenderViewAsync("/Areas/User/Views/Partials/ChatPartial.cshtml", chatVm);
            
            await _repositoryChat.SaveChangesAsync();
            await _onlineConsultantsRepository.SaveChangesAsync();
            await EstablishConnection(chat.User, newConsultant, chat.UserConnectionId);
            await Clients.Group(chat.User.UserName).SendAsync("GetNewUserChat", chatForConsultant);
        }
    } 
}
