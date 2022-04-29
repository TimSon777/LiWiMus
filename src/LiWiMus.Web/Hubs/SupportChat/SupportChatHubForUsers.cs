using LiWiMus.Core.Chats;
using LiWiMus.Core.Chats.Enums;
using LiWiMus.Core.Chats.Specifications;
using LiWiMus.Core.Messages;
using LiWiMus.Core.OnlineConsultants;
using LiWiMus.Core.OnlineConsultants.Specifications;
using LiWiMus.Core.Users;
using LiWiMus.Core.Users.Specifications;
using LiWiMus.Web.Areas.User.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LiWiMus.Web.Hubs.SupportChat;

public partial class SupportChatHub : Hub
{
    private async Task<Chat> ConnectUserWithExistChat(User user, Chat chat)
    {
        OnlineConsultant? consultant;
        
        try
        {
            consultant = await _onlineConsultantsRepository
                .GetBySpecAsync(new OnlineConsultantByConnectionIdSpec(chat.ConsultantConnectionId));;
        }
        catch (Exception)
        {
            consultant = null;
        }

        if (consultant is null)
        {
            consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantWithMinimalWorkload());
            consultant!.Chats.Add(chat);
        }

        chat.UserConnectionId = Context.ConnectionId;
        await _repositoryChat.SaveChangesAsync();
        await EstablishConnection(user, consultant);
        await _onlineConsultantsRepository.SaveChangesAsync();
        return chat;
    }

    private async Task<Chat> ConnectUserWithoutExistChat(User user)
    {
        var consultant = await _onlineConsultantsRepository.GetBySpecAsync(new ConsultantWithMinimalWorkload());

        var chat = new Chat
        {
            User = user,
            UserConnectionId = Context.ConnectionId,
            ConsultantConnectionId = consultant!.ConnectionId
        };
        
        await _repositoryChat.AddAsync(chat);
        await _repositoryChat.SaveChangesAsync();

        await EstablishConnection(user, consultant);
        await _onlineConsultantsRepository.SaveChangesAsync();
        return chat;
    }
    
    public async Task<IActionResult> ConnectUser()
    {
        var userWithoutChats = await _userManager.GetUserAsync(Context.User);

        var user = await _userRepository.GetBySpecAsync(new UserWithChatsSpec(userWithoutChats));

        var chatOld = user!.UserChats.FirstOrDefault(c => c.Status == ChatStatus.Opened);

        var chat = await (chatOld is null
            ? ConnectUserWithoutExistChat(user)
            : ConnectUserWithExistChat(user, chatOld));
        
        var chatVm = _mapper.Map<ChatViewModel>(chat);
        
        var html = await _razorViewRenderer.RenderViewAsync("/Areas/User/Views/Partials/ChatUserPartial.cshtml", chatVm);

        return new OkObjectResult(html);
    }

    public async Task SendMessageToConsultant(string text)
    {
        var user = await _userManager.GetUserAsync(Context.User);
        var chat = await _repositoryChat.GetBySpecAsync(new OpenedChatByUserSpec(user));
        
        if (chat is null)
        {
            return;
        }
        
        chat.Messages.Add(new Message
        {
            Text = text,
            Sender = user
        });

        await _repositoryChat.SaveChangesAsync();

        await Clients.Group(user.UserName).SendAsync("SendMessageToConsultant", text, user.UserName);
    }
}